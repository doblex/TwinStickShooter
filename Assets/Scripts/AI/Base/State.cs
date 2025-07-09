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

    protected float timerAttack = 0f;

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
        Attack();
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
            Ray ray = new Ray(npc.transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, controller.VisionDistance))
            {
                if (hit.transform.CompareTag("Player"))
                { 
                    return true;
                }
            }
        }

        return false;
    }


    public COMBATRANGE CanAttack()
    {
        Vector3 direction = playerTransform.position - npc.transform.position;

        RaycastHit hit;

        if (Physics.Raycast(npc.transform.position, direction.normalized, out hit, controller.AttackDistance))
        {
            if (hit.transform != playerTransform) return COMBATRANGE.FAR;


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
        else
        {
            return COMBATRANGE.FAR;
        }
    }

    protected void Seek(Vector3 position)
    {
        if (!agent.enabled) return;

        agent.SetDestination(position);

        Debug.DrawLine(agent.transform.position + Vector3.up, position + Vector3.up, Color.red);
    }

    protected void Attack()
    {
        if (!agent.enabled) return;

        if (CanAttack() == COMBATRANGE.RANGE && !playerTransform.GetComponent<EntityPlayer>().IsDead)
        {
            agent.updateRotation = false;

            agent.transform.LookAt(playerTransform.position);

            if (timerAttack >= controller.AttackCooldown)
            { 
                Shoot();
                timerAttack = 0f;
            }

            timerAttack += Time.deltaTime;
        }
        else
        {
            agent.updateRotation = true;
        }
    }

    protected void Shoot() 
    {
        GameObject projectilePrefab = controller.ProjectilePrefab;
        Transform shootPoint = controller.ShootPos;

        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = ObjectPooling.Instance.GetOrAdd(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Projectile proj = projectile.GetComponent<Projectile>();

            proj.ResetStats();

            projectile.GetComponent<Rigidbody>().AddForce(shootPoint.forward * proj.Speed, ForceMode.Impulse);
            proj.ShowTr(true);
        }
    }

}

