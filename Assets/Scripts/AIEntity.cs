using UnityEngine;
using UnityEngine.AI;

public class AIEntity : Entity
{
    NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();
        type = PGType.AI;
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Respawn()
    {
        base.Respawn();
        Debug.Log("AI Entity has respawned.");
        agent.enabled = true;

        GetComponent<Collider>().enabled = true;

    }

    protected override void OnDeath(GameObject gameObject)
    {
        base.OnDeath(gameObject);
        Debug.Log("AI Entity has died.");
        agent.enabled = false;
        
        GetComponent<Collider>().enabled = false;
    }
}
