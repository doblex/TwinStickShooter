using System.Collections.Generic;
using UnityEngine;

public class HidingSpotManager : MonoBehaviour
{
    public static HidingSpotManager Instance;

    [SerializeField] Transform playerTransform;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] int spotsPerFrame = 3;

    CapturePoint activePoint;

    List<Transform> hidingSpots = new();
    HashSet<Transform> usedSpots = new();
    HashSet<Transform> hiddenSpots = new();

    int currentIndex = 0;

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
        PointGenerator.Instance.onPointChanged += SetActivePoint;
    }

    void Update()
    {
        if (activePoint == null) return;

        UpdateVisibilityOverTime();
    }

    public void SetActivePoint(CapturePoint newPoint)
    {
        activePoint = newPoint;

        hidingSpots = new List<Transform>(activePoint.HidingSpots);
        usedSpots.Clear();
        hiddenSpots.Clear();
        currentIndex = 0;
    }

    void UpdateVisibilityOverTime()
    {
        for (int i = 0; i < spotsPerFrame && hidingSpots.Count > 0; i++)
        {
            Transform spot = hidingSpots[currentIndex];
            currentIndex = (currentIndex + 1) % hidingSpots.Count;

            Vector3 dir = playerTransform.position - spot.position;
            float dist = dir.magnitude;

            bool isHidden = Physics.Raycast(spot.position, dir.normalized, dist, obstructionMask);

            if (isHidden)
                hiddenSpots.Add(spot);
            else
                hiddenSpots.Remove(spot);
        }
    }

    public Transform RequestHidingSpot()
    {
        float bestDistance = float.MinValue;
        Transform bestSpot = null;

        float currentDistance = 0f;

        foreach (Transform spot in hiddenSpots)
        {
            if (usedSpots.Contains(spot)) continue;

            currentDistance = Vector3.Distance(playerTransform.position, spot.position);

            if (bestDistance < currentDistance)
            {
                bestDistance = currentDistance;
                bestSpot = spot;
            }
        }

        if (bestSpot != null)
        {
            usedSpots.Add(bestSpot);
        }

        return bestSpot;
    }

    public void ReleaseHidingSpot(Transform spot)
    {
        usedSpots.Remove(spot);
    }
}
