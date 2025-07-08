using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "CampStateDef", menuName = "AI/StatesDefinitions/CampStateDef")]
public class CampStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform, BehaviourController behaviour)
    {
        return new CampState(owner, agent, animator, playerTransform, behaviour);
    }
}
