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
        agent.speed = 3.5f;

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

        if (AiManager.Instance.IsZoneCaptured)
        {
            nextState = STATE.CAMP;
            stage = EVENT.EXIT;
            return;
        }

        Vector3 targetPos = target != null ? target.position : PointGenerator.Instance.CurrentCapturePoint.GetRandomPoint();

        Seek(targetPos);
    }

    public override void Exit()
    {
        HidingSpotManager.Instance.ReleaseHidingSpot(target);

        base.Exit();
    }
}