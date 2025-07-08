using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "CaptureStateDef", menuName = "AI/StatesDefinitions/CaptureStateDef")]
public class CaptureStateDefinition : StateDefinition
{
    public override State CreateState(GameObject owner, NavMeshAgent agent, Animator animator, Transform playerTransform, BehaviourController behaviour)
    {
        return new CaptureState(owner, agent, animator, playerTransform, behaviour);
    }
}