using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CapturePoint : MonoBehaviour
{
    [SerializeField] List<Entity> EntitiesInRange;

    [SerializeField] CapturePointState state;

    LineRenderer lr;

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
        SetColor();
    }

    private void SetColor()
    {
        Material mat = null;

        switch (GetState())
        {
            case CapturePointState.Neutral:
                mat = PointGenerator.Instance.Neutral;
                break;
            case CapturePointState.Contended:
                mat = PointGenerator.Instance.Contended;
                break;
            case CapturePointState.Player:
                mat = PointGenerator.Instance.Player;
                break;
            case CapturePointState.AI:
                mat = PointGenerator.Instance.Ai;
                break;
        }

        SetMaterial(mat);
    }

    public void SetMaterial(Material material)
    {
        lr.material = material;
    }

    private CapturePointState GetState()
    {
        CapturePointState pointState = CapturePointState.Neutral;
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
        else
        {
            pointState = CapturePointState.Neutral;
        }

        return pointState;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Entity>(out Entity entity))
        {
            EntitiesInRange.Remove(entity);
        }
    }
}
