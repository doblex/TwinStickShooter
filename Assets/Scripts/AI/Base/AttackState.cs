using UnityEngine;
using UnityEngine.AI;

internal class AttackState : State
{
    public AttackState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _playerTransform, _behaviuor)
    {
        stateName = STATE.ATTACK;
    }
}