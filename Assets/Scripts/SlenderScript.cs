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
    private int destPatrolPoint;

    public Animator animator;

    Vector3 target;
    Vector3 targetDir;
    float targetAngle;

    public float health = 100;

    public bool canRun = false;

    public float runSpeedFactor = 1.5f;

    private float walkSpeed, runSpeed;

    private bool alive = true;

    private bool blocked = false;
    private bool seen = false;

    private bool run = false;
    private bool inSeenSector = false;
    private bool heard = false;
    public float sightAngle = 45;
    
    Color lineColor = Color.red;

    float shootingTimer = 1f;
    bool isShooting = false;
    bool shoot = false;

    public float shootingDistance = 25f;
    float shootAngle = 15f;

    float rotationSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        // destination = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        destPatrolPoint = Random.Range(0, patrolPoints.Length);
        walkSpeed = navMeshAgent.speed;
        runSpeed = walkSpeed * runSpeedFactor;

    }

    // Update is called once per frame
    void Update()
    {
        if (alive) {

            SetAnimations();
            
            target = player.position + Vector3.up;
            targetDir = (player.position - transform.position).normalized;
            targetAngle = Vector3.Angle(targetDir, transform.forward);

            Seen();

            if (seen) {
                if (canRun) {
                    run = true;
                    navMeshAgent.speed = runSpeed;
                }
                SetDestination(player.position);
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                    run = false;
                    Quaternion lookRotation = Quaternion.LookRotation(targetDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                }
                    
                if(navMeshAgent.remainingDistance < shootingDistance && targetAngle < shootAngle) {
                    isShooting = true;
                    run = false;
                    Shoot();
                } else {
                    isShooting = false;
                }
                
            } else {
                if (canRun) {
                    run = false;
                    navMeshAgent.speed = walkSpeed;
                }
                isShooting = false;
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 1f) {
                    GotoRandomPoint();
                }
            }

            // draw debug line
            DebugDraw();


        } else {
            navMeshAgent.enabled = alive;
            animator.SetBool("alive", alive);
        }

    }

    void SetAnimations(){
        if(navMeshAgent.velocity != Vector3.zero){
            animator.SetBool("idle", false);
        } else {
            animator.SetBool("idle", true);
        }

        animator.SetBool("isShooting", isShooting);
        animator.SetBool("seen", seen);
        if (canRun) {
            animator.SetBool("run", run);
        }
    }

    void Seen() {
        if (targetAngle < sightAngle) {
            inSeenSector = true;
            NavMeshHit hit;
            blocked = navMeshAgent.Raycast(target, out hit);
                if (!blocked) {
                    seen = true;
                } else {
                    seen = false;
                }
        } else {
            inSeenSector = false;
            seen = false;
        }
    }


    void DebugDraw() {
        lineColor = Color.red;
        Debug.DrawLine(transform.position, target, lineColor);
        if (inSeenSector) {
            lineColor = Color.grey;
            Debug.DrawLine(transform.position + new Vector3(0.1f, 0, 0), target, lineColor);
        }

        if (heard) {
            lineColor = Color.yellow;
            Debug.DrawLine(transform.position + new Vector3(0.2f, 0, 0), target, lineColor);
        }
        
        if (seen) {
            lineColor = Color.green;
            Debug.DrawLine(transform.position + new Vector3(0.3f, 0, 0), target, lineColor);
        }

        if (heard & seen) {
            lineColor = Color.blue;
            Debug.DrawLine(transform.position + new Vector3(0.4f, 0, 0), target, lineColor);
        }
    }

    void Shoot() {
        if (shoot) {
            return;
        }
    

        StartCoroutine(Shoot_Coroutine());

        if (Random.Range(0, 100) <= 50) {
            if (inSeenSector && !blocked) {
            //    print("enemy HITS player");
                PlayerController pc = player.GetComponentInParent<PlayerController>();
                pc.TakeDamage(10f);
            }
        } else {
         //   print("enemy MISS");
        }
    }

    IEnumerator Shoot_Coroutine() {
        shoot = true;
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(shootingTimer);
        shoot = false;
        if(alive)
        {
            navMeshAgent.isStopped = false;
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

    void OnTriggerStay(Collider collider) {
        if(alive && collider.gameObject.tag == "Player") {
            SetDestination(player.position);
            heard = true;
        }
    }

    public void TakeDamage(string _collider, float _weaponDamage)
    {
        if(_collider == "Bone004" || _collider == "pää001")
        {
            health = 0;
        } else
        {
            health -= _weaponDamage;
        }

        if(health <= 0)
        {
            alive = false;
        }
    }
}
