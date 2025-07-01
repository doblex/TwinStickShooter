using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;

    private Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
           
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject GetOrAdd(GameObject prefab, bool setPosition)
    {
        return GetOrAdd(prefab, Vector3.zero, Quaternion.identity, setPosition);
    }

    public GameObject GetOrAdd(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return GetOrAdd(prefab, position, rotation, true);
    }

    private GameObject GetOrAdd(GameObject prefab, Vector3 position, Quaternion rotation, bool setPosition)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new List<GameObject>();
        }

        // Look for an inactive object
        foreach (var obj in pools[prefab])
        {
            if (!obj.activeInHierarchy)
            {
                if(setPosition)
                    obj.transform.SetPositionAndRotation(position, rotation);

                obj.SetActive(true);
                return obj;
            }
        }

        // If none found, instantiate new
        GameObject newObj = Instantiate(prefab, position, rotation);
        pools[prefab].Add(newObj);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ReturnToPool(GameObject obj, float delay)
    { 
        StartCoroutine(ReturnToPoolAfterDelay(obj, delay));
    }

    IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(obj);
    }
}
