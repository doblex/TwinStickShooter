using System.Collections.Generic;
using UnityEngine;
using utilities.Controllers;

[RequireComponent(typeof(LineRenderer))]
public class CapturePoint : MonoBehaviour
{
    public delegate void OnpointCaptured(CapturePointState capturePointState);

    public event OnpointCaptured onPointCaptured;

    [SerializeField] List<Entity> EntitiesInRange;
    [SerializeField] float captureTimeToCapture = 5f;
    [SerializeField] float pointAwardTime = 1f;

    [SerializeField] List<Transform> hidingSpots = new();

    LineRenderer lr;

    [SerializeField][ReadOnly]CapturePointState currentState;
    CapturePointState previusState = CapturePointState.Neutral; 
    bool isCaptured;
    bool firedEvent = false;

    float captureTime = 0f;
    float awardTime = 0f;

    public List<Transform> HidingSpots { get => hidingSpots; }
    public CapturePointState CurrentState { get => currentState; }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lr.loop = true;
        lr.enabled = true;
        lr.positionCount = 4;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        Collider collider = GetComponent<Collider>();

        Vector3 colliderSize = collider.bounds.extents;
        Vector3 center = collider.bounds.center;
        float height = collider.bounds.max.y;

        Vector3[] points = new Vector3[4];

        points[0] = center + new Vector3(-colliderSize.x, height, -colliderSize.z);
        points[1] = center + new Vector3(-colliderSize.x, height, colliderSize.z);
        points[2] = center + new Vector3(colliderSize.x, height, colliderSize.z);
        points[3] = center + new Vector3(colliderSize.x, height, -colliderSize.z);
        
        lr.SetPositions(points);

        SetMaterial(currentState);
    }

    private void Update()
    {
        CheckState();
        AwardPoints();
    }

    private void AwardPoints()
    {
        if (!isCaptured) return;

        if (awardTime >= pointAwardTime)
        {
            switch (currentState)
            {
                case CapturePointState.PlayerCaptured:
                    PointsManager.Instance.AddPlayerPoints(1);
                    break;
                case CapturePointState.AICaptured:
                    PointsManager.Instance.AddAiPoints(1);
                    break;
            }
            Debug.Log($"Points Awarded: {currentState} at {gameObject.name}");
            awardTime = 0f;
        }

        awardTime += Time.deltaTime;
    }

    private void CheckState()
    {
        currentState = GetState();

        isCaptured = captureTime >= captureTimeToCapture;

        bool isStateChanged = currentState != previusState;

        if (isStateChanged)
        {
            if ((previusState == CapturePointState.PlayerCaptured && currentState == CapturePointState.AICaptured)
                || (previusState == CapturePointState.AICaptured && currentState == CapturePointState.PlayerCaptured)
                || (currentState != CapturePointState.PlayerCaptured && currentState != CapturePointState.AICaptured)
                )
            { 
                captureTime = 0f;
            }

            firedEvent = false;
            previusState = currentState;
            SetMaterial(currentState);
            Debug.Log($"Capture Point State Changed: {currentState} at {gameObject.name}");
        }

        if (currentState != CapturePointState.Neutral && currentState != CapturePointState.Contended)
            captureTime += Time.deltaTime;
    }

    public void SetMaterial(CapturePointState state)
    {
        if (state == CapturePointState.Neutral)
        {
            lr.material = PointGenerator.Instance.Neutral;
        }
        else if (state == CapturePointState.Contended)
        {
            lr.material = PointGenerator.Instance.Contended;
        }
        else if (state == CapturePointState.Player)
        {
            lr.material = PointGenerator.Instance.Player;
        }
        else if (state == CapturePointState.PlayerCaptured)
        {
            lr.material = PointGenerator.Instance.PlayerCaptured;
        }
        else if (state == CapturePointState.AI)
        {
            lr.material = PointGenerator.Instance.Ai;
        }
        else if (state == CapturePointState.AICaptured)
        {
            lr.material = PointGenerator.Instance.AiCaptured;
        }

        Debug.Log($"Capture Point Material Set: {lr.material.name} for state {state} at {gameObject.name}");
    }

    private CapturePointState GetState()
    {
        CapturePointState pointState = previusState;
        bool isAIPresent = false;
        bool isPlayerPresent = false;

        isCaptured = captureTime >= captureTimeToCapture;

        foreach (Entity entity in EntitiesInRange)
        {
            switch (entity.Type)
            {
                case PGType.Player:
                    isPlayerPresent = true;
                    break;
                case PGType.AI:
                    isAIPresent = true;
                    break;
            }
        }

        if (isAIPresent && isPlayerPresent)
        {
            pointState = CapturePointState.Contended;
        }
        else if (isPlayerPresent && !isAIPresent)
        {
            if (isCaptured)
            {
                pointState = CapturePointState.PlayerCaptured;

                if (!firedEvent)
                {
                    onPointCaptured?.Invoke(CapturePointState.Player);
                    firedEvent = true;
                    Debug.Log($"Capture Point Captured by Player at {gameObject.name}");
                }
            }
            else
            { 
                pointState = CapturePointState.Player;
            }
        }
        else if (!isPlayerPresent && isAIPresent)
        {
            if (isCaptured)
            {
                pointState = CapturePointState.AICaptured;

                if (!firedEvent)
                {
                    onPointCaptured?.Invoke(CapturePointState.AI);
                    firedEvent = true;
                    Debug.Log($"Capture Point Captured by AI at {gameObject.name}");
                }
            }
            else
            {
                pointState = CapturePointState.AI;
            }
        }

        return pointState;
    }

    public void ResetCapturePoint() 
    {
        previusState = CapturePointState.Neutral;
        captureTime = 0f;
        EntitiesInRange.Clear();

        Debug.Log($"Capture Point Reset at {transform.position} to state {previusState}.");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Entity>(out Entity entity))
        {
            EntitiesInRange.Add(entity);
            other.gameObject.GetComponent<HealthController>().onDeath += OnDeath;
            if(other.gameObject.TryGetComponent<BehaviourController>(out BehaviourController controller))
                controller.OnCapturePoint = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Entity>(out Entity entity))
        {
            EntitiesInRange.Remove(entity);
            other.gameObject.GetComponent<HealthController>().onDeath -= OnDeath;
            if (other.gameObject.TryGetComponent<BehaviourController>(out BehaviourController controller))
                controller.OnCapturePoint = false;
        }
    }

    private void OnDeath(GameObject gameObject)
    {
        Entity entity = gameObject.GetComponent<Entity>();

        if (entity != null)
        {
            EntitiesInRange.Remove(entity);
            gameObject.GetComponent<HealthController>().onDeath -= OnDeath;
            if (gameObject.TryGetComponent<BehaviourController>(out BehaviourController controller))
                controller.OnCapturePoint = false;
        }
    }

    public Vector3 GetRandomPoint()
    {
        Collider collider = GetComponent<Collider>();

        Vector3 point = transform.position;
        Vector3 colliderSize = collider.bounds.extents;

        float randomX = Random.Range(-colliderSize.x, colliderSize.x);
        float randomZ = Random.Range(-colliderSize.z, colliderSize.z);

        point.x += randomX;
        point.z += randomZ;

        return point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (Transform spot in hidingSpots)
        {
            Gizmos.DrawSphere(spot.position, 0.2f);
        }
    }
}
