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

    public delegate void RespawnTimer(RespawnObj obj, int type); // 0 for timerUpdate , 1 for respawned , 2 for Enqueue 

    public event RespawnTimer RespawnTimerEvent;

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

            if (obj.Entity.Type == PGType.Player)
            {
                RespawnTimerEvent?.Invoke(obj,0);
            }

            if (obj.Timer <= 0)
            {
                if (obj.Entity.Type == PGType.Player)
                {
                    RespawnTimerEvent?.Invoke(obj, 1);
                }

                obj.Entity.Respawn();
                entitiesToSpawn.RemoveAt(i);
                i--;
            }
        }
    }

    public void RespawnEntity(Entity entity)
    {
        

        RespawnObj obj = new RespawnObj()
        {
            Entity = entity,
            Timer = RespawnTime
        };

        if (obj.Entity.Type == PGType.Player)
        {
            RespawnTimerEvent?.Invoke(obj, 2);
        }

        entitiesToSpawn.Add(obj);
    }
}
