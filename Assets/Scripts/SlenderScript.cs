using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlenderScript : MonoBehaviour
{
    // GameObject destination;
    NavMeshAgent navMeshAgent;

    Transform player;

    public bool spawnPatrol = true;
    public Transform[] spawnPatrolPoints;
    private int destSpawnPatrolPoint;
    public Transform[] patrolPoints;
    private int destPatrolPoint;

    public Animator animator;
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public GameObject audioPrefab;
    public GameObject healthMonitor;
    private Material monitorMat;

    Vector3 target;
    Vector3 targetDir;
    float targetAngle;

    public float health = 100;

    public bool canRun = false;

    public float runSpeedFactor = 1.5f;

    private float walkSpeed, runSpeed;

    bool goAtDirection = false;

    public float followDirectionLenght = 20f;

    private bool alive = true;

    private bool blocked = false;

    Vector3 seenPosition;

    Vector3 seenDirection;
    
    private bool seen = false;

    private bool run = false;
    private bool inSeenSector = false;
    private bool heard = false;
    private bool heardCollider = false;

    bool lookAt = false;
    bool lookAtNoise = false;
    bool look = false;

    float lookingTimer = 1f;

    Vector3 heardPosition;

    public float hearDistance = 100f;
    public float sightAngle = 45;
    public float sightDistance = 50f;
    
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
        player = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        destPatrolPoint = Random.Range(0, patrolPoints.Length);
        destSpawnPatrolPoint = 0;
        walkSpeed = navMeshAgent.speed;
        runSpeed = walkSpeed * runSpeedFactor;
        monitorMat = healthMonitor.GetComponent<Renderer>().material;


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
            Heard();
            if(lookAt) {
                LookAt(targetDir);
            } else if (seen) {
                spawnPatrol = false;
                goAtDirection = true;
                if (canRun) {
                    run = true;
                    navMeshAgent.speed = runSpeed;
                }
                SetDestination(seenPosition);
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                    run = false;
                    lookAt = true;
                }
                    
                if(navMeshAgent.remainingDistance < shootingDistance && targetAngle < shootAngle) {
                    isShooting = true;
                    run = false;
                    Shoot();
                } else {
                    isShooting = false;
                }
                
            } else if (heard) {
                SetDestination(heardPosition);
                lookAtNoise = true;
                heard = false;
            } else {
                if (canRun) {
                    run = false;
                    navMeshAgent.speed = walkSpeed;
                }
                isShooting = false;
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 1f) {
                    if(lookAtNoise) {
                        lookAt = true;
                        lookAtNoise = false;
                    } else if(goAtDirection) {
                        Vector3 goAt = transform.position + Vector3.Scale(seenDirection, new Vector3(followDirectionLenght, 0, followDirectionLenght));
                        navMeshAgent.SetDestination(goAt);
                        goAtDirection = false;
                    } else if(spawnPatrol) {
                        GotoNextPoint();
                    } else {
                        GotoRandomPoint();
                    }
                    
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
                if (!blocked && hit.distance < sightDistance) {
                    seen = true;
                    if (seenPosition != player.position) {
                        seenDirection = (player.position - seenPosition).normalized;
                        seenPosition = player.position;
                    }
                } else {
                    seen = false;
                }
        } else {
            inSeenSector = false;
            seen = false;
        }
    }

    void Heard() {
        if (heardCollider) {
            NavMeshPath path = new NavMeshPath();
            if(navMeshAgent.enabled) {
                navMeshAgent.CalculatePath(player.position, path);
            }

            Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

            allWayPoints[0] = transform.position;
            allWayPoints[allWayPoints.Length - 1] = player.position;

            for(int i=0; i<path.corners.Length; i++) {
                allWayPoints[i+1] = path.corners[i];
            }

            float pathLenght = 0f;

            for(int i=allWayPoints.Length-1; i>0; i--) {
                pathLenght += Vector3.Distance(allWayPoints[i], allWayPoints[i-1]);
                // if (pathLenght <= hearDistance) {
                //     heardPosition = allWayPoints[i];
                //     heard = true;
                // }
            }

            // print("allWayPointsLenght: " + allWayPoints.Length + ", hearing pathLenght: " +  pathLenght + ", heardDistance: " + hearDistance);

            if (pathLenght <= hearDistance) {
                if(allWayPoints.Length >= 5) {
                    heardPosition = allWayPoints[4];
                } else {
                    heardPosition = player.position;
                }
                
                heard = true;
            }
        }
    }


    void DebugDraw() {
        lineColor = Color.red;
        Debug.DrawLine(transform.position, target, lineColor);
        Debug.DrawLine(transform.position, navMeshAgent.pathEndPosition, Color.white);
        if (inSeenSector) {
            lineColor = Color.grey;
            Debug.DrawLine(transform.position + new Vector3(0.1f, 0, 0), target + new Vector3(0.01f, 0, 0), lineColor);
        }

        if (heardCollider) {
            lineColor = Color.yellow;
            Debug.DrawLine(transform.position + new Vector3(-0.1f, 0, 0), target + new Vector3(-0.01f, 0, 0), lineColor);
        }
        
        if (seen) {
            lineColor = Color.green;
            Debug.DrawLine(transform.position + new Vector3(0.2f, 0, 0), target + new Vector3(0.02f, 0, 0), lineColor);
        }

        if (heard) {
            lineColor = Color.blue;
            Debug.DrawLine(transform.position + new Vector3(-0.2f, 0, 0), heardPosition, lineColor);
        }
    }

    void LookAt(Vector3 direction) {
        

        if (look) {
            
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            return;
        }

        StartCoroutine(Look_Coroutine());

    }
    
    IEnumerator Look_Coroutine() {
        look = true;
        yield return new WaitForSeconds(lookingTimer);
        look = false;
        if(alive)
        {
            lookAt = false;
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
        PlayMuzzleFlash();
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
        if (spawnPatrolPoints.Length == 0) {
            spawnPatrol = false;
            return;
        }
            

        // Set the agent to go to the currently selected destination.
        navMeshAgent.destination = spawnPatrolPoints[destSpawnPatrolPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destSpawnPatrolPoint = (destSpawnPatrolPoint + 1) % spawnPatrolPoints.Length;
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
            heardCollider = true;
            if(collider.GetType() == typeof(CapsuleCollider)) {
                CapsuleCollider c = (CapsuleCollider)collider;
                if (hearDistance < c.radius) {
                    hearDistance = c.radius;
                    // print(collider.name+":name, hearDistance:"+hearDistance);
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (alive && collider.gameObject.tag == "Player")
        {
            heardCollider = false;
            hearDistance = 0f;
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
            StartCoroutine(DamageIndicator_Coroutine());
        }

        if (health <= 0)
        {
            monitorMat.SetColor("_EmissionColor", Color.red);
        //    GetComponentInChildren<Material>().SetColor("_EmissionColor", Color.red);
            alive = false;
            transform.GetComponent<Rigidbody>().isKinematic = false;
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.AddForce(player.forward * 50f, ForceMode.Impulse);
            Animator animator = transform.GetComponentInChildren<Animator>();
            animator.enabled = false;
        }
    }

    public void SetSpawnPatrolPoints(Transform[] spp) {
        spawnPatrolPoints = spp;
        spawnPatrol = true;
    }

    void PlayMuzzleFlash()
    {
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(shootSound, false, 0.1f);
        muzzleFlash.Play();
    }

    IEnumerator DamageIndicator_Coroutine()
    {
        monitorMat.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.2f);
        monitorMat.SetColor("_EmissionColor", Color.yellow);

    }
}
