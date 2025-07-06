using UnityEngine;

[RequireComponent(typeof(PlayerShoot), typeof(PlayerInputController))]
public class EntityPlayer : Entity
{
    public delegate void OnLockInput(bool locked); 

    public OnLockInput lockInput;

    protected override void Awake()
    {
        base.Awake();
        type = PGType.Player;
    }

    public override void Respawn()
    {
        base.Respawn();
        lockInput?.Invoke(false);
    }

    protected override void OnDeath(GameObject gameObject)
    {
        base.OnDeath(gameObject);
        lockInput?.Invoke(true);
    }
}
