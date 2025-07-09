using System.Threading;
using UnityEngine;
using UnityEngine.AI;

internal class CampState : State
{
    Transform hidingSpot;
    float timer = 0;

    public CampState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _playerTransform, _behaviuor)
    {
        stateName = STATE.CAMP;
    }

    public override void Enter()
    {
        base.Enter();
        agent.updatePosition = true;
        timer = 0f;
        agent.stoppingDistance = 0f;
        agent.speed = 3f;
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

        if (timer >= 4f)
        {
            timer = 0f;
            HidingSpotManager.Instance.ReleaseHidingSpot(hidingSpot);
            hidingSpot = HidingSpotManager.Instance.RequestHidingSpot();

            if (hidingSpot != null)
            {
                Seek(hidingSpot.position);
            }
            else
            {
                Seek(PointGenerator.Instance.CurrentCapturePoint.GetRandomPoint());
            }
        }

        timer += Time.deltaTime;
    }
}