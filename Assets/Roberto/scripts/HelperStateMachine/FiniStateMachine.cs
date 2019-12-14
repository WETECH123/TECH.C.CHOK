using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniStateMachine : MonoBehaviour,IFroze
{
    
   public AIState currentState;
    AIState pastState;
    Dictionary<stateType, AIState> validStates;
    NavMeshAgent agent;
    KitchenHelper helper;
    //internal bool canMove;
    private bool isDead;
    B_Player player;
    Animator anim;
    public GameObject pancakePiece;
    public GameObject WeaponModel;
   float frozingTime = 5;
   public bool isfrozen { get; set;}
    float invincibleCounter = 5;
    [SerializeField]
    ParticleSystem effect = null;
    bool isInvincible=false;
   
    //bool charge = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        helper = GetComponent<KitchenHelper>();
        player = helper.getPlayer();
        WeaponModel.SetActive(false);
        EventsInitializing();
        STATUS status = helper.status;
        validStates = new Dictionary<stateType, AIState>();
        validStates.Add(stateType.idle, new IdleSM(this.agent, this, helper, status, stateType.idle, anim));
        validStates.Add(stateType.attack, new AttackSM(this.agent, this, helper, status, stateType.attack, anim));
        validStates.Add(stateType.defence, new DefenceSM(this.agent, this, helper, status, stateType.defence, anim));
        validStates.Add(stateType.cake, new MakeCakeSM(this.agent, this, helper, status, stateType.cake, anim));
        currentState = validStates[stateType.defence];
        pastState = validStates[stateType.cake];
        changeState(stateType.idle);
        isfrozen = false;
    }

   
    void Update()
    {
        if (isDead) return;
        if (isfrozen)
        {
            froze();
            return;
        }
        updateAnimator();
        currentState.UPDATING();
        if (isInvincible)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                invincibleCounter = 5;
                becomeInvincible(false);
            }
        }
    }

    internal void changeState(stateType statetype)
    {
        pastState = currentState;
        currentState.ExitState();
        if (validStates.ContainsKey(statetype))
        {
            currentState = validStates[statetype];
        }
        else
        {
            Debug.LogError("don't have this state assigned");
        }
        if(pastState.stateType != statetype||statetype==stateType.attack)
        {
            currentState.EnterState();

        }
      
    }

    void updateAnimator()
    {
        Vector3 velocity = agent.velocity;
        anim.SetFloat(StaticStrings.move, velocity.magnitude);
        anim.SetBool(StaticStrings.HAVEMATERIAL, helper.haveMaterial);
        pancakePiece.SetActive(helper.haveMaterial);
    }
    void EventsInitializing()
    {
        HealthManager health = GetComponent<HealthManager>();
        if (health != null)
        {
            health.onDeath += onDeath;
        }

        player.onChangeOrder += changeState;
    }
    public void onDeath(bool value)
    {
        isDead = value;
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger(StaticStrings.death);
    }

    public void WeaponCollider(int v)
    {
        bool value;
       if (v == 1)
        {
            value = true;
        }
        else
        {
            value = false;
        }
        WeaponModel.gameObject.GetComponentInChildren<Collider>().enabled = value;
    }


   

    public  string randomAttack()
    {
        string atk;
        int num = UnityEngine.Random.Range(1,helper.status.getCurrentweapon.numOfattack);
        switch (num)
        {
            case 1:
                atk = StaticStrings.attack1;
                break;
            case 2:
                atk = StaticStrings.attack2;
                break;
            case 3:
                atk = StaticStrings.attack3;
                break;
            default:
                atk = StaticStrings.attack1;
                break;
        }
        return atk;
    }
    private void froze()
    {
        frozingTime -= Time.deltaTime;
        agent.SetDestination(transform.position);
        if (frozingTime <= 0)
        {
            isfrozen = false;
            frozingTime = 5;
        }
    }

    public void frozing()
    {
        isfrozen = true;
    }
    public void becomeInvincible(bool v)
    {
        if (effect == null) return;
        isInvincible = v;
        if (v)
            effect.Play();
        else
            effect.Stop();

        HealthManager h = GetComponent<HealthManager>();
        if (h)
        {
            h.becameInvincible(v);
        }
    }
}
