using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "CureStateDef", menuName = "AI/StatesDefinitions/CureStateDef")]
public class CureStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform, BehaviourController behaviour)
    {
        return new CureState(owner, agent, animator, playerTransform, behaviour);
    }
}