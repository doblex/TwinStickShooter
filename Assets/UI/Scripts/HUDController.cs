using System;
using UnityEngine;
using UnityEngine.UIElements;
using utilities.Controllers;

public class HUDController : DocController
{
    ProgressBar aiPointsBar;
    ProgressBar playerPointsBar;

    VisualElement playerLife;

    Label invincibleLabel;
    bool isInvincible = false;

    Label reaspawnCounter;

    protected override void SetComponents()
    {
        aiPointsBar = Root.Q<ProgressBar>("AI");
        playerPointsBar = Root.Q<ProgressBar>("Player");

        playerLife = Root.Q<VisualElement>("LifeBarFill");

        invincibleLabel = Root.Q<Label>("Invincible");

        reaspawnCounter = Root.Q<Label>("RespawnCounter");
    }

    private void Start()
    {
        aiPointsBar.value = 0;
        playerPointsBar.value = 0;

        aiPointsBar.highValue = PointsManager.Instance.PointsToWin;
        playerPointsBar.highValue = PointsManager.Instance.PointsToWin;

        reaspawnCounter.style.display = DisplayStyle.None;

        UpdatePoints(0, 0);

        playerLife.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
        GameObject.FindWithTag("Player").GetComponent<HealthController>().onDamage += UpdatePlayerLife;

        PointsManager.Instance.OnPointsUpdated += UpdatePoints;

        RespawnManager.Instance.RespawnTimerEvent += OnRespawnTimerEvent;
    }

    private void OnRespawnTimerEvent(RespawnManager.RespawnObj obj, int type)
    {

        reaspawnCounter.text = $"{Math.Round(obj.Timer, 0)}s";

        switch (type)
        {
            case 1:
                reaspawnCounter.text = "Respawned!";
                reaspawnCounter.style.display = DisplayStyle.None;
                break;
            case 2:
                reaspawnCounter.style.display = DisplayStyle.Flex;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isInvincible = !isInvincible;
            GameObject.FindWithTag("Player").GetComponent<HealthController>().SetInvicible(isInvincible);
            Debug.Log($"Player Invincibility: {isInvincible}");

            invincibleLabel.style.color = isInvincible ? new StyleColor(Color.green) : new StyleColor(Color.red);
        }
    }

    private void UpdatePoints(int playerPoints, int aiPoints)
    {
        aiPointsBar.value = (float) aiPoints;
        playerPointsBar.value = (float)playerPoints;
    }

    private void UpdatePlayerLife(float MaxHp, float currentHp)
    {
        float percent = (currentHp / MaxHp) * 100;

        playerLife.style.height = new StyleLength(new Length(Math.Clamp(percent, 0, 100), LengthUnit.Percent));
    }
}
