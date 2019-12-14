using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SA;

namespace td
{
    public class TD_InputHandler : MonoBehaviour
    {
        float horizontal;
        float vertical;
        public event Action<Transform> onGiveTarget;
        bool isInit;
        B_Player player;
        float delta;
        public TD_StateManager states;
        TD_CameraHandler camerahandler;
        Animator anim;
        [SerializeField]
        Transform hand = null;
        [SerializeField]
        WPbullet bomb = null;
        
        
        

        public bool isfrozen { get; set; }

        bool L2Input()
        {
            return Input.GetButton(StaticStrings.L2_key);

        }

        public void InitInGame()
        {
            anim = GetComponent<Animator>();
            player = GetComponentInChildren<B_Player>();
            
            //dependency with statemanager
            states.init();
            camerahandler = FindObjectOfType<TD_CameraHandler>();
            camerahandler.Init(this);
            isInit = true;
           
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
            vertical = Input.GetAxis(StaticStrings.Vertical);
            horizontal = Input.GetAxis(StaticStrings.Horizontal);

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

        }
        #endregion
        #region Update
        void Update()
        {
            if (!isInit)
                return;
            
            delta = Time.deltaTime;
            states.tick(delta);
            crossAirUpdate();
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
            WPbullet b = Instantiate(bomb, hand.position, hand.rotation);
            Vector3 dir;
            if (Input.GetButton(StaticStrings.L2_key))
                dir = player.getCameraDir();
            else
            {
                dir = transform.forward * 50;
            }
            b.gameObject.GetComponent<Rigidbody>().AddForce(dir * 50);
        }
    }
}


