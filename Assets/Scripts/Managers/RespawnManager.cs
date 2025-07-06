using System;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Serializable]
    public class RespawnObj 
    {
        public Entity Entity;
        public float Timer;
    }

    public static RespawnManager Instance;

    [SerializeField] float RespawnTime = 5f;
    [SerializeField] List<RespawnObj> entitiesToSpawn = new List<RespawnObj>();


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

    private void Update()
    {
        for (int i = 0; i < entitiesToSpawn.Count; i++)
        {
            RespawnObj obj = entitiesToSpawn[i];

            obj.Timer -= Time.deltaTime;

            if (obj.Timer <= 0)
            {
                obj.Entity.Respawn();
                entitiesToSpawn.RemoveAt(i);
                i--;
            }
        }
    }

    public void RespawnEntity(Entity entity)
    {
        entitiesToSpawn.Add(
            new RespawnObj()
            {
                Entity = entity,
                Timer = RespawnTime
            });
    }
}
