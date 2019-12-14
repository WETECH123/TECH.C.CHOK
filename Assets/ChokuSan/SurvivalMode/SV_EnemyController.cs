using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SV_EnemyController : MonoBehaviour
{   
    public Transform player;               // Reference to the player's position.
    SV_PlayerHealth playerHealth;      // Reference to the player's health.
    SV_EnemyHealth enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.


    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<SV_PlayerHealth>();
        enemyHealth = GetComponent<SV_EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        // If the enemy and the player have health left...
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
        // ... set the destination of the nav mesh agent to the player.
        nav.SetDestination(player.position);
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
           nav.enabled = false;
        }
    }
}