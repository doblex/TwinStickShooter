using UnityEngine;
using UnityEngine.AI;

internal class CaptureState : State
{
    Transform target;

    public CaptureState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _playerTransform, _behaviuor)
    {
        stateName = STATE.CAPTURE;
    }

    public override void Enter()
    {
        base.Enter();
        agent.updatePosition = true;
        agent.stoppingDistance = 0f;
        agent.speed = 2f;

        target = HidingSpotManager.Instance.RequestHidingSpot();
    }

    public override void Update()
    {
        base.Update();

        if (AiManager.Instance.IsZoneContested)
        {
            nextState = STATE.CHASE;
            stage = EVENT.EXIT;
            return;
        }

        Seek(target != null ? target.position : PointGenerator.Instance.CurrentCapturePoint.GetRandomPoint());
    }

    public override void Exit()
    {
        HidingSpotManager.Instance.ReleaseHidingSpot(target);

        base.Exit();
    }
}