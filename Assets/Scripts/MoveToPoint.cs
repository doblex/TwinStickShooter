using UnityEngine;
using UnityEngine.AI;

public class MoveToPoint : MonoBehaviour
{
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(agent.enabled)
            agent.SetDestination(PointGenerator.Instance.CurrentCapturePoint.GetPosition());
    }
}
