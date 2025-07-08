using System.Collections.Generic;
using UnityEngine;

public class HealManager : MonoBehaviour
{
    public static HealManager Instance;

    [SerializeField] List<HealPackController> healPacks = new();


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

    public Vector3 GetNearestHeal(Vector3 position)
    {
        Vector3 nearestPos = Vector3.zero;
        float distance = Mathf.Infinity;
        float curDistance = 0f;

        foreach (var heal in healPacks)
        {
            if (heal.IsTaken) continue;

            curDistance = Vector3.Distance(heal.transform.position, position);

            if (distance > curDistance)
            {
                distance = curDistance;
                nearestPos = heal.transform.position;
            }
        }

        return nearestPos;
    }
}
