using UnityEngine;
using UnityEngine.AI;
using utilities.Controllers;

public class CureState : State
{
    public CureState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor) : base(_npc, _agent, _anim, _playerTransform, _behaviuor)
    {
        stateName = STATE.CURE;
    }

    public override void Enter()
    {
        base.Enter();
        agent.stoppingDistance = 0f;
        agent.speed = 4.5f;
    }

    public override void Update()
    {
        base.Update();
        Seek(HealManager.Instance.GetNearestHeal(npc.transform.position));

        if (npc.GetComponent<HealthController>().CurrentHp > controller.HealThreshold)
        {
            nextState = STATE.IDLE;
            stage = EVENT.EXIT;
        }
    }
}