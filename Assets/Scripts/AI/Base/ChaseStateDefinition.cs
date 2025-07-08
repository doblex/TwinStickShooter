using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChaseStateDef", menuName = "AI/StatesDefinitions/ChaseStateDef")]
public class ChaseStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform, BehaviourController behaviour)
    {
        return new ChaseState(owner, agent, animator, playerTransform, behaviour);
    }
}