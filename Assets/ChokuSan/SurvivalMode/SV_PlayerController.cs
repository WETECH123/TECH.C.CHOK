using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using td;
public class SV_PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
   [SerializeField]
    private float forwardMoveSpeed = 7.5f;
    [SerializeField]
    private float backwardMoveSpeed = 3;
    [SerializeField]
    private float turnSpeed = 15f;
    float speedMultipler = 100;
    float fireDalyCounter = 0.8f;
    float fireDelay = 0.8f;
    TD_BulletSpawner bulletSpawner;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        bulletSpawner = GetComponentInChildren<TD_BulletSpawner>();
       // Cursor.lockState = CursorLockMode.Locked;
    }
 
    void Update()
    {
       
        var horizontal = Input.GetAxis(StaticStrings.cameraX);
        var vertical = Input.GetAxis(StaticStrings.Vertical);

        var movement = new Vector3(horizontal, 0, vertical);

        animator.SetFloat(StaticStrings.move, vertical);

        transform.Rotate(Vector3.up, horizontal * turnSpeed*speedMultipler * Time.deltaTime);
       
        if (vertical != 0)
        {
            float moveSpeedToUse = (vertical > 0) ? forwardMoveSpeed : backwardMoveSpeed;
            characterController.SimpleMove(transform.forward * moveSpeedToUse * vertical);

        }
      
      if (Input.GetButton(StaticStrings.R2_key))
        {
            fireDalyCounter -= Time.deltaTime;
            if (fireDalyCounter <= 0)
            {
                fireDalyCounter = fireDelay;
                animator.SetTrigger(StaticStrings.shooting);
            }
            return;
        }
    }

    
    public void shot()
    {
        bulletSpawner.normalShoot(transform.forward);
    }
}
