using System;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public delegate void PointsUpdated(int playerPoints, int aiPoints);

    public event PointsUpdated OnPointsUpdated;

    public static PointsManager Instance;

    [Header("Points")]
    [SerializeField] int playerPoints = 0;
    [SerializeField] int aiPoints = 0;
    [SerializeField] int pointsToWin = 100;

    public int PointsToWin { get => pointsToWin; }

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
    }

    private void ResetPoints()
    {
        playerPoints = 0;
        aiPoints = 0;
    }

    public void AddPlayerPoints(int points)
    {
        playerPoints += points;
        Debug.Log($"Player Points: {playerPoints}");
        OnPointsUpdated?.Invoke(playerPoints, aiPoints);

        if (playerPoints >= pointsToWin)
        {
            UIManager.Instance.ShowEndGameDoc(true);
        }
    }

    public void AddAiPoints(int points)
    {
        aiPoints += points;
        Debug.Log($"AI Points: {aiPoints}");
        OnPointsUpdated?.Invoke(playerPoints, aiPoints);

        if (aiPoints >= pointsToWin)
        {
            UIManager.Instance.ShowEndGameDoc(false);
        }
    }
}
