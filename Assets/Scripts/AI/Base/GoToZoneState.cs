using UnityEngine;
using UnityEngine.AI;

internal class GoToZoneState : State
{
    public GoToZoneState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _playerTransform, _behaviuor)
    {
        stateName = STATE.GO_TO_ZONE;
    }
}