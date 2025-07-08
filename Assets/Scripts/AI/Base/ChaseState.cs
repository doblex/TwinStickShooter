using UnityEngine;
using UnityEngine.AI;

internal class ChaseState : State
{
    public ChaseState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _playerTransform, _behaviuor)
    {
        stateName = STATE.CHASE;
    }

    public override void Enter()
    {
        base.Enter();
        agent.updatePosition = true;
        agent.stoppingDistance = 5f;
        agent.speed = 2.2f;
    }

    public override void Update()
    {
        base.Update();

        Seek(playerTransform.position);
    }
}