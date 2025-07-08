using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PointGenerator : MonoBehaviour
{
    public static PointGenerator Instance;

    public delegate void OnPointChanged(CapturePoint capturePoint);
    public event OnPointChanged onPointChanged;

    [Header("Materials")]
    [SerializeField] Material neutral;
    [SerializeField] Material contended;
    [SerializeField] Material player;
    [SerializeField] Material playerCaptured;
    [SerializeField] Material ai;
    [SerializeField] Material aiCaptured;

    [SerializeField] List<CapturePoint> points;

    [Header("Options")]
    [SerializeField] float changePointDelay = 40f;

    [ReadOnly][SerializeField] CapturePoint currentCapturePoint = null;
    float changepointTimer = 0;

    public Material Neutral { get => neutral; }
    public Material Contended { get => contended; }
    public Material Player { get => player; }
    public Material Ai { get => ai; }
    public CapturePoint CurrentCapturePoint { get => currentCapturePoint; }
    public Material PlayerCaptured { get => playerCaptured ; }
    public Material AiCaptured { get => aiCaptured; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetPoints();
        ChangePoint();
    }

    private void Update()
    {
        CheckChangePoint();
    }

    private void ResetPoints()
    {
        foreach (CapturePoint point in points)
        {
            point.gameObject.SetActive(false);
        }
    }

    private void CheckChangePoint()
    {
        if (changepointTimer <= 0)
        { 
            changepointTimer = changePointDelay;
            ChangePoint();
        }

        changepointTimer -= Time.deltaTime;
    }

    private void ChangePoint()
    {
        bool isFound = false;
        while (!isFound)
        {
            int rndIdx = Random.Range(0, points.Count);

            if (points[rndIdx] != currentCapturePoint)
            {
                SetCurrentPoint(points[rndIdx]);
                isFound = true;
            }
        }
    }

    private void SetCurrentPoint(CapturePoint point)
    {
        if(currentCapturePoint != null)
            currentCapturePoint.gameObject.SetActive(false);

        currentCapturePoint = point;
        currentCapturePoint.gameObject.SetActive(true);
        currentCapturePoint.ResetCapturePoint();

        onPointChanged?.Invoke(currentCapturePoint);

    }

    public void RegisterEvent(CapturePoint.OnpointCaptured func)
    { 
        foreach (CapturePoint point in points)
        {
            point.onPointCaptured += func;
        }
    }

    public void UnregisterEvent(CapturePoint.OnpointCaptured func)
    {
        foreach (CapturePoint point in points)
        {
            point.onPointCaptured -= func;
        }
    }
}
