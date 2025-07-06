using System.Collections.Generic;
using UnityEngine;
using utilities.Controllers;

[RequireComponent(typeof(LineRenderer))]
public class CapturePoint : MonoBehaviour
{
    [SerializeField] List<Entity> EntitiesInRange;
    [SerializeField] float captureTimeToCapture = 5f;
    [SerializeField] float pointAwardTime = 1f;


    LineRenderer lr;

    CapturePointState currentState;
    CapturePointState previusState = CapturePointState.Neutral;
    bool isCaptured;

    float captureTime = 0f;
    float awardTime = 0f;


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
                case CapturePointState.Player:
                    PointsManager.Instance.AddPlayerPoints(1);
                    break;
                case CapturePointState.AI:
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

        bool isStateChanged = currentState != previusState;

        if (isStateChanged)
        {
            captureTime = 0f;
            previusState = currentState;
            Debug.Log($"Capture Point State Changed: {currentState} at {gameObject.name}");
        }

        isCaptured = captureTime >= captureTimeToCapture;

        SetMaterial(currentState);

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
            if (isCaptured)
            {
                lr.material = PointGenerator.Instance.PlayerCaptured;
            }
            else
            {
                lr.material = PointGenerator.Instance.Player;
            }
        }
        else if (state == CapturePointState.AI)
        {
            if (isCaptured)
            {
                lr.material = PointGenerator.Instance.AiCaptured;
            }
            else
            {
                lr.material = PointGenerator.Instance.Ai;
            }
        }

        Debug.Log($"Capture Point Material Set: {lr.material.name} for state {state} at {gameObject.name}");
    }

    private CapturePointState GetState()
    {
        CapturePointState pointState = previusState;
        bool isAIPresent = false;
        bool isPlayerPresent = false;

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
            pointState = CapturePointState.Player;
        }
        else if (!isPlayerPresent && isAIPresent)
        {
            pointState = CapturePointState.AI;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Entity>(out Entity entity))
        {
            EntitiesInRange.Remove(entity);
            other.gameObject.GetComponent<HealthController>().onDeath -= OnDeath;
        }
    }

    private void OnDeath(GameObject gameObject)
    {
        Entity entity = gameObject.GetComponent<Entity>();

        if (entity != null)
        {
            EntitiesInRange.Remove(entity);
            gameObject.GetComponent<HealthController>().onDeath -= OnDeath;
        }
    }
}
