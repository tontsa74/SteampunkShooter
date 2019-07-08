using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlenderScript : MonoBehaviour
{
    // GameObject destination;
    NavMeshAgent navMeshAgent;

    public Transform eyes;

    public Transform player;

    public Transform[] patrolPoints;
    private int destPatrolPoint;

    public Animator animator;


    public float health = 100;

    private bool alive = true;

    private bool blocked = false;
    private bool seen = false;
    public float sightAngle = 45;


    // Start is called before the first frame update
    void Start()
    {
        // destination = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        destPatrolPoint = Random.Range(0, patrolPoints.Length);
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

            Vector3 target = player.position + Vector3.up;
            NavMeshHit hit;
            blocked = navMeshAgent.Raycast(target, out hit);
            
            Debug.DrawLine(transform.position, target, blocked || !seen ? Color.red : Color.green);

            Vector3 targetDir = player.position - eyes.position;
            float angle = Vector3.Angle(targetDir, eyes.forward);
            if (angle < sightAngle) {
                seen = true;
            } else {
                seen = false;
            }



            if (!blocked && seen) {
                //transform.LookAt(player.position);
                
                SetDestination(player.position);
            } else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 1f) {
                GotoRandomPoint();
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

    void GotoRandomPoint() {
        // Returns if no points have been set up
        if (patrolPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        navMeshAgent.destination = patrolPoints[destPatrolPoint].position;

        
        // Choose the next random point in the array as the destination
        destPatrolPoint = Random.Range(0, patrolPoints.Length);
    }

    public void TakeDamage(string _collider, float _weaponDamage)
    {
        if(_collider == "Bone004")
        {
            health -= _weaponDamage * 10;
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
