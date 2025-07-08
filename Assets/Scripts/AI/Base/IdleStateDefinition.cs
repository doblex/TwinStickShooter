using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName ="IdleStateDef", menuName = "AI/StatesDefinitions/IdleStateDef")]
public class IdleStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform, BehaviourController behaviour)
    {
        return new IdleState(owner, agent, animator, playerTransform, behaviour);
    }
}
