using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public NavMeshAgent agent;

    public Transform playerTransform;

    public LayerMask Ground;
    public LayerMask Player;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float sightRange;
    public bool playerInSightRange;

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);

        if (!playerInSightRange)
        {
            Patroling();
        }
        else
        {
            Chasing();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomz = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomz);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground)) walkPointSet = true;
    }

    private void Chasing()
    {
        agent.SetDestination(playerTransform.position);
    }
}