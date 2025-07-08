using UnityEngine;
using utilities.Controllers;

[RequireComponent(typeof(HealthController))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected Transform respawn;
    [SerializeField] GameObject mesh;

    protected HealthController healthController;

    protected PGType type;
    public PGType Type { get => type; }

    public bool IsDead { get => isDead; }

    bool isDead = false;

    protected virtual void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    protected virtual void Start()
    {
        healthController.onDeath += OnDeath;
    }

    protected virtual void OnDeath(GameObject gameObject) 
    {
        mesh.SetActive(false);
        isDead = true;
        RespawnManager.Instance.RespawnEntity(this);
    }

    public virtual void Respawn() 
    {
        mesh.SetActive(true);
        isDead = false;
        transform.position = respawn.position;
        healthController.ResetHealth();
    }
}
