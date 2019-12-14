using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
   
     public class Inputhandler : MonoBehaviour,IFroze,IConfused
     {
        float horizontal;
        float vertical;
        PlayerActions playerActions;
        public event Action<Transform> onGiveTarget;
        bool isInit;
        B_Player player;
        float delta;
        public StatesManager states;
        Camerahandler camerahandler;
        Animator anim;
        BulletSpawner bs;
        [SerializeField]
        GameObject weaponModel=null, SpecialWeaponModel=null;
        [SerializeField]
        Transform hand=null;
        [SerializeField]
        WPbullet bomb=null;
        float frozingTime = 5;
        float confusedTime = 10;
        bool isConfused = false;

        string ver="", hor="";
         
        public bool isfrozen { get; set;}

        bool L2Input()
        {
            return Input.GetButton(StaticStrings.L2_key);

        }

        public void InitInGame()
            {
            ver= StaticStrings.Vertical;
            hor = StaticStrings.Horizontal;
            anim = GetComponent<Animator>();
            player = GetComponentInChildren<B_Player>();
            player.initializeCanvas();
            //dependency with statemanager
            states.init();
            camerahandler = GetComponentInChildren<Camerahandler>();
            camerahandler.Init(this);
            playerActions = new PlayerActions(player, this,anim);
            player.Helper.INITIALIZETARGETEVENT(this);
            reciveTrget(playerActions.targets[0]);
            bs = GetComponentInChildren<BulletSpawner>();
            bs.INITIALIZING(player);
            player.reciveAnimator(anim);
            isInit = true;
            player.assignWeapons(weaponModel, SpecialWeaponModel);
            isfrozen = false;
        }

            #region Fixed Update

            void FixedUpdate()
            {
                if (!isInit)
                    return;
                delta = Time.fixedDeltaTime;
                GetInput_FixedUpdate();
                InGame_UpdateStates_FixedUpdate();
                states.FixedTick(delta);
                camerahandler.fixedTick(delta);
                
        }

            void GetInput_FixedUpdate()
            {
                vertical = Input.GetAxis(ver);
                horizontal = Input.GetAxis(hor);
                
            }

            void InGame_UpdateStates_FixedUpdate()
            {
                states.inp.horizontal = horizontal;
                states.inp.vertical = vertical;

                states.inp.moveAmount = (Mathf.Clamp01(Mathf.Abs(horizontal)) + Mathf.Abs(vertical));

                Vector3 moveDir = camerahandler.mTransform.forward * vertical;
                moveDir += camerahandler.mTransform.right * horizontal;
                moveDir.Normalize();
                states.inp.moveDirection = moveDir;

                states.inp.rotateDirection = camerahandler.mTransform.forward;
            if (isConfused)
            {
                confusedTime -= Time.deltaTime;
                if (confusedTime <= 0)
                {
                    isConfused = false;
                    ver = StaticStrings.Vertical;
                    hor = StaticStrings.Horizontal;
                }
            }
        }
            #endregion
            #region Update
            void Update()
            {
                if (!isInit)
                    return;
            if (isfrozen)
            {
                froze();
                return;
            }
                delta = Time.deltaTime;
                states.tick(delta);
                crossAirUpdate();
                playerActions.InputUpdating();
            
        }

        private void froze()
        {
            frozingTime -= Time.deltaTime;
            if (frozingTime <= 0)
            {
                isfrozen = false;
                frozingTime = 5;
            }
        }

        public void shot()
        {

            player.BULLETSPAWNER.shot();

        }

        void crossAirUpdate()
        {
            if (!states.onGround()) return;
            states.states.isAiming = L2Input();
         
        }
        #endregion
      

        public void reciveTrget(Transform t)
        {
              if (onGiveTarget != null)
            {
                onGiveTarget(t);
            }
        }

        public void frozing()
        {
            isfrozen = true;
        }

        public void spawnBomb()
        {
          WPbullet b=  Instantiate(bomb, hand.position, hand.rotation);
            Vector3 dir;
            if (Input.GetButton(StaticStrings.L2_key))
                dir = player.getCameraDir();
            else
            {
                dir = transform.forward * 50;
            }
            b.gameObject.GetComponent<Rigidbody>().AddForce(dir*50);
        }

        public void confuse()
        {
            confusedTime = 10;
            isConfused = true;
            ver = StaticStrings.Horizontal;
            hor = StaticStrings.Vertical;
        }
    }
    
        public enum GamePhase
        {
            inGame, inMenu,
        }

    
}

