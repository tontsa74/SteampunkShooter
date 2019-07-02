using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlenderScript : MonoBehaviour
{
    // GameObject destination;
    NavMeshAgent navMeshAgent;

    public Transform target;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // destination = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.velocity != Vector3.zero){
            animator.SetBool("idle", false);
        } else {
            animator.SetBool("idle", true);
        }
        // Sight();
        NavMeshHit hit;
        if (!navMeshAgent.Raycast(target.position, out hit)) {
            SetDestination();
        }
    }

    public void SetDestination()
    {
        // navMeshAgent.SetDestination(destination.transform.position);
        navMeshAgent.SetDestination(target.position);
    }

}
