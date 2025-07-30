using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO LIST
// 1. Implement vision cone for sight detection
// 2. Timer for when chase period ends if player gets away
// 3. change size of vision cone based on chase vs no chase vs player speed/crouch etc.

public class ChasePlayer : MonoBehaviour
{

    public Transform player;
    private NavMeshAgent agent;
    //public Transform visionCone;

    public bool sight = true;
    public bool hearing = true;
    public bool light_sensory = true;

    private bool isChasing = false;
    private Vector3 currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(RandomMovement());
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            agent.destination = player.position;
        }
    }

    private IEnumerator RandomMovement()
    {
        while (!isChasing)
        {
            if (!agent.hasPath || agent.remainingDistance < 0.5f)
            {
                currentTarget = GetRandomPointOnNavMesh();
                agent.SetDestination(currentTarget);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            isChasing = true;
            Debug.Log("Player entered the vision");
        }
    }
}
