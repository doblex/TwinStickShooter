using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "AttackStateDef", menuName = "AI/StatesDefinitions/AttackStateDef")]
public class AttackStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform, BehaviourController behaviour)
    {
        return new AttackState(owner, agent, animator, playerTransform, behaviour);
    }
}