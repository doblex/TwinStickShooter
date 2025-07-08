using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public IdleState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _playerTransform, _behaviuor)
    {
        stateName = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
        agent.updatePosition = false;
    }

    public override void Update()
    {
        base.Update();
        if (controller.IsDead) return; 

        if (!AiManager.Instance.IsTeamCamping)
        {
            nextState = STATE.CAPTURE;
            stage = EVENT.EXIT;
        }
        else
        {
            nextState = STATE.CHASE;
            stage = EVENT.EXIT;
        }
    }
}