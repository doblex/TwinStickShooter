using UnityEngine;
using UnityEngine.AI;

public abstract class StateDefinition : ScriptableObject
{
    public STATE stateName;
    public abstract State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform ,BehaviourController behaviour);
}

public class GoToZoneStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform, BehaviourController behaviour)
    {
        return new GoToZoneState(owner, agent, animator, playerTransform, behaviour);
    }
}