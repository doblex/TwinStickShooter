using System.Collections;
using UnityEngine;
using utilities.Controllers;

public class HealPackController : MonoBehaviour
{
    [SerializeField] int healthAmount = 5;
    [SerializeField] float respawnTime = 10f;
    [SerializeField] GameObject mesh;

    bool isTaken = false;

    public bool IsTaken { get => isTaken; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthController controller))
        {
            controller.RestoreHealth(healthAmount);
            mesh.SetActive(false);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(Respawn());
            isTaken = true;
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        mesh.SetActive(true);
        GetComponent<Collider>().enabled = true;
        isTaken = false;
    }
}
