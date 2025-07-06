using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using utilities.Controllers;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AnimatorController))]
[RequireComponent(typeof(HealthController))]
public class BehaviourController : MonoBehaviour
{
    [SerializeField] Animator animator;
    NavMeshAgent agent;
    HealthController healthController;

    [Header("States")]
    [SerializeField] STATE defaultState;
    [SerializeField]List<StateDefinition> stateDefinitions = new List<StateDefinition>();

    [Header("Vision")]
    [SerializeField] float visionDistance = 15.0f;
    [SerializeField] float visionAngle = 60.0f;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 2.0f;

    [Header("Attack")]
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField][ShowIf("isRanged", false)] int attackDamage = 1;
    [SerializeField, Range(1f, 20f)] float attackDistance = 4.0f;
    [SerializeField, Range(1f, 20f)] float minimumRange = 0.0f;
    [SerializeField] bool isRanged = false;
    [SerializeField][ShowIf("isRanged")] GameObject projectilePrefab;
    [SerializeField][ShowIf("isRanged")] Transform shootPos;
    [SerializeField][ShowIf("isRanged")] float projectileSpeed = 10.0f;

    [Header("Speeds")]
    [SerializeField] float wanderSpeed = 1f;
    [SerializeField] float chaseSpeed = 2f;
    [SerializeField] float repositionSpeed = 1f;

    [Header("DON'T TOUCH")]
    [ReadOnly][SerializeField] List<State> states = new List<State>();
    [ReadOnly][SerializeField] State currentState;

    Transform player;

    #region Getters
    public State CurrentState { get => currentState; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }
    public Transform ShootPos { get => shootPos; }
    public float VisionDistance { get => visionDistance; }
    public float VisionAngle { get => visionAngle; }
    public float MinimumRange { get => minimumRange; }
    public float AttackDistance { get => attackDistance; }
    public bool IsRanged { get => isRanged; }
    public float ProjectileSpeed { get => projectileSpeed; }
    public float AttackCooldown { get => attackCooldown; }
    public int AttackDamage { get => attackDamage; }
    public float RotationSpeed { get => rotationSpeed; }
    public float WanderSpeed { get => wanderSpeed; }
    public float ChaseSpeed { get => chaseSpeed; }
    public float RepositionSpeed { get => repositionSpeed; }
    #endregion


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthController = GetComponent<HealthController>();
        healthController.onDeath += OnDeath;
    }

    private void OnDeath(GameObject gameObject)
    {
        ChangeState(STATE.DEATH);      
    }

    private void Update()
    {
        if (currentState != null)
        {
            ChangeState(currentState.Process());
        }
        else
        { 
            ChangeState(defaultState);
        }
    }

    public void ChangeState(STATE stateName)
    {
        State previousState = currentState;
        currentState = GetOrAddState(stateName);

        if(currentState != previousState)
            previousState.stage = EVENT.ENTER;
    }

    private State GetOrAddState(STATE stateName)
    {
        GetState(stateName, out State retrievedState);

        return retrievedState;
    }

    public void RemoveState(STATE stateName)
    {
        foreach (var state in states)
        {
            if (state.stateName == stateName)
            {
                states.Remove(state);
                return;
            }
        }
    }

    public bool HasStateDefinition(STATE stateName)
    {
        foreach (var state in stateDefinitions)
        {
            if (state.stateName == stateName)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasState(STATE stateName)
    {
        foreach (var state in states)
        {
            if (state.stateName == stateName)
            {
                return true;
            }
        }
        return false;
    }

    private void GetState(STATE stateName, out State retrievedState)
    {
        foreach (var state in states)
        {
            if (state.stateName == stateName)
            {
                retrievedState = state;
                return;
            }
        }

        retrievedState = GetStateFromDefinition(stateName);
        states.Add(retrievedState);
    }

    private State GetStateFromDefinition(STATE stateName)
    {
        foreach (var stateDefinition in stateDefinitions)
        {
            if (stateDefinition.stateName == stateName)
            {
                return stateDefinition.CreateState(gameObject, GetComponent<NavMeshAgent>(), animator, player, this);
            }
        }

        Debug.LogError($"StateDefinition for {stateName} not found.");
        return null;
    }
}

