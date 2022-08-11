using UnityEngine;
using UnityEngine.AI;
public class AiController : MonoBehaviour
{
    public NavMeshAgent agent;

    [Range(0, 100)] private float speed;
    [Range(0, 500)] public float walkRadius;

    // Start is called before the first frame update
    void Start()
    {
        //custom movement for all npcs
        speed = Random.Range(0.1f, 0.25f);

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed;
            agent.SetDestination(RandomNavMeshLocation());
        }
    }

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        randomPosition.y = 0f;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }


    // Update is called once per frame
    void Update()
    {
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(RandomNavMeshLocation());
        }
    }
}
