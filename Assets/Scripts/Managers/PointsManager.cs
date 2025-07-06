using System;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance;

    [Header("Points")]
    [SerializeField] int playerPoints = 0;
    [SerializeField] int aiPoints = 0;
    [SerializeField] int pointsToWin = 100;

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

        if (playerPoints >= pointsToWin)
        {
            Debug.Log("Player wins!");
        }
    }

    public void AddAiPoints(int points)
    {
        aiPoints += points;
        Debug.Log($"AI Points: {aiPoints}");

        if (aiPoints >= pointsToWin)
        {
            Debug.Log("AI wins!");
        }
    }
}
