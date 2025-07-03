using UnityEngine;

[RequireComponent(typeof(PlayerShoot), typeof(PlayerInputController))]
public class EntityPlayer : Entity
{
    public delegate void OnLockInput(bool locked); 

    public OnLockInput lockInput;

    [SerializeField] GameObject mesh;

    protected override void Awake()
    {
        base.Awake();
        type = PGType.Player;
    }

    public override void Respawn()
    {
        mesh.SetActive(true);
        lockInput?.Invoke(true);
    }

    protected override void OnDeath()
    {
        mesh.SetActive(false);
        lockInput?.Invoke(false);
    }
}
