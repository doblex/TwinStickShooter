using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode aimKey = KeyCode.Mouse1;

    [Header("Shooting settings")]
    [SerializeField] float shootCooldown = 0.5f; // Cooldown time in seconds
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint;

    [Header("Cam settings")]
    [SerializeField] Transform aimLock;
    [SerializeField] float aimLockDistance = 10f;

    float timer;
    bool lockedInput;

    private void Awake()
    {
        GetComponent<EntityPlayer>().lockInput += Locked;
    }

    private void Locked(bool locked)
    {
        lockedInput = locked;
    }

    private void Update()
    {
        if (lockedInput) return;


        if (Input.GetKey(shootKey))
        {
            if (timer <= 0f)
            {
                Shoot();
                timer = shootCooldown;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(aimKey))
        {
            CameraManager.Instance.SwitchCamera(1);
        }

        if (Input.GetKeyUp(aimKey))
        { 
            CameraManager.Instance.SwitchCamera(0);
        }

        MoveAimLock();
    }

    private void MoveAimLock()
    {
        Camera cam = CameraManager.Instance.GetActiveCamera();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            Vector3 flatHitPoint = new Vector3(hitInfo.point.x, 0f, hitInfo.point.z);
            Vector3 directionFromPlayer = flatHitPoint - transform.position;
            directionFromPlayer = Vector3.ClampMagnitude(directionFromPlayer, aimLockDistance);

            aimLock.position = transform.position + directionFromPlayer;
        }
    }


    private void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = ObjectPooling.Instance.GetOrAdd(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Projectile proj = projectile.GetComponent<Projectile>();

            proj.ResetStats();

            projectile.GetComponent<Rigidbody>().AddForce(shootPoint.forward * proj.Speed, ForceMode.Impulse);
            proj.ShowTr(true);
        }
    }
}
