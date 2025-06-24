using UnityEngine;
using UnityEngine.AI;

public abstract class StateDefinition : ScriptableObject
{
    public STATE stateName;
    public abstract State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform ,BehaviourController behaviour);
}
