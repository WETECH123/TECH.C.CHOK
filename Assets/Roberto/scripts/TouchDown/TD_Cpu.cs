using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
namespace td
{
    public class TD_Cpu : TD_Character
    {
        
        [SerializeField]
        float atkRange = 15;
        [SerializeField]
        float atkCounter = 1, atkDelay = 1;
        Transform target = null;
        NavMeshAgent agent;
        cpuStat stat = cpuStat.chasing;
        List<Transform> enemyList = new List<Transform>();
        Vector3 targetpos = Vector3.zero;
        Transform door;
        Vector3 doorPos = Vector3.zero;
        TD_BulletSpawner bulletSpawner = null;
        TD_TeamDirector td;

        public override void Init()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
           TD_PancakeBall.instance.onTakingBall += reciveTarget;
            bulletSpawner = GetComponentInChildren<TD_BulletSpawner>();
            findTarget();
           
            Invoke("lateInit", 2.5f);

        }
        
       void lateInit()
        {
            TD_GameManager gm = FindObjectOfType<TD_GameManager>();
            gm.setResetableObjects(this.gameObject);
            if (gm.playerA.t != team)
            {
                door = gm.playerA.door;
            }
            else
            {
                door = gm.playerB.door;
            }
            
            doorPos = new Vector3(door.transform.position.x, transform.position.y, door.transform.position.z);
            TD_PancakeBall.instance.onTakingBall += reciveTarget;
            base.Init();
        }
      

        void findTarget()
        {
           
          TD_Character[]  enemies = FindObjectsOfType<TD_Character>();
         foreach( var i in enemies)
            {
                if (i.team != team)
                {
                    enemyList.Add(i.transform);
                }
            }
            if (enemyList.Count > 0)
            {
                enemyList.OrderBy(c => Vector3.Distance(transform.position, c.transform.position));
                target = enemyList[0];
            }        
        }

        public override void updating()
        {
            base.updating();
              switch (stat)
            {
                case cpuStat.chasing:
                    agent.SetDestination(targetpos);
                    distanceCheck();
                    break;
                case cpuStat.attack:
                    atkBeahviour();
                    break;
                case cpuStat.runUp:
                    agent.SetDestination(doorPos);
                    break;
            }
            anim.SetFloat(StaticStrings.move, agent.velocity.magnitude);
            if (target == null)
            {
                if (stat != cpuStat.runUp)
                {
                    stat = cpuStat.runUp;
                }
            }
        }

        void distanceCheck()
        {
            if (target == null) return;
            targetpos = new Vector3(target.position.x, transform.position.y, target.position.z);
            float distance = Vector3.Distance(transform.position, targetpos);
            if (distance <= atkRange)
            {
                stat = cpuStat.attack;
            }
            Debug.DrawLine(this.transform.position, target.transform.position + new Vector3(0, 3, 0), Color.red);
        }
        /// <summary>
        ///  attack selected target
        /// </summary>
        public void atkBeahviour()
        {
            if (target == null) return;
            targetpos = new Vector3(target.position.x, transform.position.y, target.position.z);
            float distance = Vector3.Distance(transform.position, targetpos);
            if (distance > atkRange)
            {
                stat = cpuStat.chasing;
            }
            if (agent.destination != transform.position)
            {
                agent.SetDestination(transform.position);
            }
            transform.LookAt(target, Vector3.up);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            atkCounter -= Time.deltaTime;
            if (atkCounter <= 0)
            {
                atkCounter = atkDelay;
                anim.SetTrigger(StaticStrings.shooting);
            }
        }
        public void reciveTarget(Team tm,Transform tg)
        {
            if(tm!=this.team)
            target = tg;
            changeStatus(cpuStat.chasing);
        }
        public override void haveBall(bool v)
        {
            base.haveBall(v);
            target = null;    
        }
        public override void reset()
        {
            base.reset();
            findTarget();
            stat = cpuStat.chasing;
        }

        public void AIshoot()
        {
            bulletSpawner.aiShot(transform.forward*100);
        }
      
        public void changeStatus(cpuStat s)
        {
            agent.ResetPath();
            stat = s;
        }
        public override void respawning()
        {
            transform.position = startPosition;
        }
        public enum cpuStat
        {
            chasing,
            attack,
            runUp
        }
    }
}

