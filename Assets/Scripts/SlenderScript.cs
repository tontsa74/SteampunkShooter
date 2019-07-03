using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlenderScript : MonoBehaviour
{
    // GameObject destination;
    NavMeshAgent navMeshAgent;

    public Transform player;

    public Transform[] patrolPoints;
    private int destPatrolPoint = 0;

    public Animator animator;


    public float health = 100;

    private bool alive = true;


    // Start is called before the first frame update
    void Start()
    {
        // destination = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive) {
            if(navMeshAgent.velocity != Vector3.zero){
                animator.SetBool("idle", false);
            } else {
                animator.SetBool("idle", true);
            }
            // Sight();
            NavMeshHit hit;

            if (!navMeshAgent.Raycast(player.position, out hit)) {
                SetDestination(player.position);
            } else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f) {
                GotoNextPoint();
            }    
        } else {
            navMeshAgent.enabled = alive;
            animator.SetBool("alive", alive);
        }

    }

    public void SetDestination(Vector3 pos)
    {
        navMeshAgent.SetDestination(pos);
    }

    void GotoNextPoint() {
        // Returns if no points have been set up
        if (patrolPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        navMeshAgent.destination = patrolPoints[destPatrolPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPatrolPoint = (destPatrolPoint + 1) % patrolPoints.Length;
    }

    public void TakeDamage(string _collider, float _weaponDamage)
    {
        if(_collider == "Bone004")
        {
            health -= _weaponDamage * 2;
        } else
        {
            health -= _weaponDamage;
        }

        if(health <= 0)
        {
            print("SlenderScript: DEAD");
            alive = false;
        }

        print("SlenderScript: health "+health);
    }
}
