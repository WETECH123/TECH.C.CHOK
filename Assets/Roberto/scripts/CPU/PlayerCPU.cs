using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;


public class PlayerCPU
{
    Animator anim;
    [Header("settings")]
    [SerializeField]
    public float targetCheckTimer = 10;
    float targetCheckCounter;
    public Transform target = null;
    Vector3 direction = Vector3.zero;
    public string State = "IDLE";
    B_Player player;
    NavMeshAgent agent;
    STATUS status;
    float waiting = 1.5f;
    Transform thisTransform;
    float attackTimer;
    bool skillActive = false;

    float skillTimer = 10;
    #region Boleans
    bool finding = false;

    bool shooting()
    {
        return State == StaticStrings.shooting;
    }

    bool idle()
    {
        return State == StaticStrings.idle;
    }

    bool run()
    {
        return State == StaticStrings.running;
    }
    #endregion

    public PlayerCPU(Animator an, B_Player p, NavMeshAgent ag, STATUS st, Transform thisTran)
    {
        status = st;
        anim = an;
        targetCheckCounter = targetCheckTimer;
        player = p;
        agent = ag;
        attackTimer = status.getCurrentweapon.attackdelay;
        thisTransform = thisTran;
        target = player.enemyList[0].transform;
        player.manaIsFull += onManaIsFull;
        player.takeIngredient += useIngredient;
        
    }

    public void UpdateStatus()
    {
       
        if (player.returnDeath() == true) {
            agent.isStopped = true;
            agent.ResetPath();
            finding = false;
            waiting = 1.5f;
            State = StaticStrings.idle;
        }
        
        targetCheckCounter -= Time.deltaTime;
        if (targetCheckCounter <= 0)
        {
            targetCheckCounter = targetCheckTimer;
            State = StaticStrings.idle;
        }
        if (idle())
        {
            stay();
        }
        if (run())
        {
            chasing();
        }
        if (shooting())
        {
            fight();
        }
        updateAnimator();
        if (skillActive)
        {
            skillUpdating(Time.deltaTime);
        }
    }

    private void skillUpdating(float deltaTime)
    {
        skillTimer -= deltaTime;
        if (skillTimer <= 0)
        {
            player.status.UseManapoint(100);
            skillActive = false;
            player.activeSkill(false);
            skillTimer = 10;
        }
    }

    void updateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = thisTransform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        anim.SetFloat(StaticStrings.move, speed);
        anim.SetBool(StaticStrings.respawned, !player.returnDeath());

    }
    #region battleStatus
    void stay()
    {
        if (agent.isStopped)
            agent.isStopped = false;

        if (agent.destination != thisTransform.position)
        {
            agent.SetDestination(thisTransform.position);
        }
        if (!finding)
        {
            waiting -= Time.deltaTime;
            if (waiting <= 0)
            {
                waiting = 1.5f;
                finding = true;
                findNewTarget();
                agent.SetDestination(target.position);
                State = StaticStrings.running;
                finding = false;
            }

        }
    }
    void chasing()
    {
        
            float distance = Vector3.Distance(thisTransform.position, target.position);
        if (distance <= status.getCurrentweapon.attackrange)
        {
            agent.SetDestination(thisTransform.position);
            State = StaticStrings.shooting;
        }
        else if(distance > status.getCurrentweapon.attackrange &&distance< status.getCurrentweapon.chasingdistance)
        {
            
            agent.SetDestination(target.position);
        }
    }
    void fight()
    {
        float distance = Vector3.Distance(thisTransform.position, target.position);
        thisTransform.LookAt(target, Vector3.up);
        thisTransform.rotation = Quaternion.Euler(0, thisTransform.eulerAngles.y, 0);
        if (distance <= status.getCurrentweapon.attackrange)
        {
            agent.SetDestination(player.transform.position);

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                Vector3 loockDir = target.position - player.transform.position;
                attackTimer = status.getCurrentweapon.attackdelay;
                 player.BULLETSPAWNER.GiveDirection(loockDir);
                if (player.status.BOMB < 1)
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName(StaticStrings.shooting) && !anim.IsInTransition(0))
                        anim.SetTrigger(StaticStrings.shooting);
                    return;
                }
                else
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName(StaticStrings.shooting) && !anim.IsInTransition(0))
                        anim.SetTrigger(StaticStrings.bomb);
                    player.status.BOMB--;
                }
   
            }
            
        }
        else
        {
            State = StaticStrings.running;
            attackTimer = status.getCurrentweapon.attackdelay;
        }
    }
    #endregion

    #region ENEMYTARGET RESOURCE
    public void findNewTarget()
    {
        int a = UnityEngine.Random.Range(1, 4);
        switch (a)
        {
            case 1: choosePlayerTarget(); break;
            case 2: chooseTowerTarget(); break;
            case 3: chooseHelperTarget(); break;
            default: break;
        }
    }

    public void choosePlayerTarget()
    {

        target = giveRandomTarget().transform;

    }
    public void chooseHelperTarget()
    {
        target = giveRandomTarget().Helper.transform;
    }
    public void chooseTowerTarget()
    {

        target = giveRandomTarget().Tower.transform;
    }

    public void reciveTarget(Transform initialTarget)
    {
        target = initialTarget;
    }

    B_Player giveRandomTarget()
    {
        int choose = UnityEngine.Random.Range(0, player.enemyList.Capacity - 1);
        return player.enemyList[choose];
    }
    #endregion

    void onManaIsFull()
    {
        if (!skillActive)
        {
            skillActive = true;
            player.activeSkill(true);
            
        }
    }
   
    #region ingredients
    void useIngredient()
    {

        int rand = UnityEngine.Random.Range(1, 4);
        switch (rand)
        {
            case 1:PowerUp_Tower(5);
                break;
            case 2: PowerUp_Add_SkillPoints(50);
                break;
            case 3: PowerUp_Helper(5);
                break;
        }
        player.status.Ingredients--;  
    }

    
    void PowerUp_Helper(float value)
    {
        player.Helper.powerUp(value);
    }
    void PowerUp_Tower(float value)
    {
        player.Tower.powerUp(value);
    }
    void PowerUp_Add_SkillPoints(float value)
    {
        player.Add_SkillPoints(value);
    }
    #endregion
}
