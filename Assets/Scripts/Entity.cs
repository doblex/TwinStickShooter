using UnityEngine;
using utilities.Controllers;

[RequireComponent(typeof(HealthController))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected Transform respawn;

    protected HealthController healthController;

    protected PGType type;
    public PGType Type { get => type; }

    protected virtual void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    protected virtual void Start()
    {
        healthController.onDeath += OnDeath;
    }

    protected abstract void OnDeath();

    public abstract void Respawn();
}
