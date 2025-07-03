using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public class RespawnObj 
    {
        public Entity Entity;
        public float Timer;
    }

    [SerializeField] float RespawnTime = 5f;
    List<RespawnObj> entitiesToSpawn = new List<RespawnObj>();
    

    public void RespawnEntity(Entity entity)
    {
        entitiesToSpawn.Add(
            new RespawnObj()
            {
                Entity = entity,
                Timer = RespawnTime
            });
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
            }
        }
    }
}
