using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class State
{
    public STATE stateName;
    public EVENT stage;

    protected STATE nextState;

    protected GameObject npc;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected BehaviourController controller;
    protected Transform playerTransform;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _playerTransform, BehaviourController _behaviuor)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        playerTransform = _playerTransform;
        controller = _behaviuor;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() 
    {
        stage = EVENT.UPDATE;
    }
    public virtual void Update() 
    {
        stage = EVENT.UPDATE;
    }
    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public STATE Process()
    {
        if (stage == EVENT.ENTER) Enter();
        else if(stage == EVENT.UPDATE) Update();
        else if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return stateName;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = playerTransform.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if(direction.magnitude < controller.VisionDistance && angle < controller.VisionAngle)
        {
            return true;
        }

        return false;
    }


    public COMBATRANGE CanAttack()
    {
        Vector3 direction = playerTransform.position - npc.transform.position;

        if (direction.magnitude > controller.AttackDistance + 1)
        {
            return COMBATRANGE.FAR;
        }
        else if (direction.magnitude < controller.MinimumRange)
        {
            return COMBATRANGE.CLOSE;
        }
        else
        {
            return COMBATRANGE.RANGE;
        }
    }

    public bool IsPlayerBehind()
    {
        Vector3 direction = npc.transform.position - playerTransform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        float dotProduct = Vector3.Dot(npc.transform.forward, playerTransform.forward);
        
        
        if (direction.magnitude < controller.AttackDistance && angle < 30)
        {
            return true;
        }

        return false;
    }

    protected void Seek(Vector3 position)
    {
        agent.SetDestination(position);

        Debug.DrawLine(agent.transform.position + Vector3.up, position + Vector3.up, Color.red);
    }
}

