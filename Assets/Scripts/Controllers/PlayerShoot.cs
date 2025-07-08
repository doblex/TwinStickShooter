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

    [Header("laser Options")]
    [SerializeField][Range(1, 10)] float baseLaserDistance;
    [SerializeField][Range(5, 20)] float laserAimDistance;
    [SerializeField] LayerMask projectileLayer;

    [Header("Cam settings")]
    [SerializeField] Transform aimLock;
    [SerializeField] float aimLockDistance = 10f;

    float timer;
    bool lockedInput;

    LineRenderer lr;
    float laserDistance;

    private void Awake()
    {
        GetComponent<EntityPlayer>().lockInput += Locked;
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lr.startWidth = 0.05f;
        laserDistance = baseLaserDistance;
    }

    private void Locked(bool locked)
    {
        lockedInput = locked;
        lr.enabled = !locked;
    }

    private void Update()
    {
        KeyCheck();
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        if (lr != null && lr.enabled)
        {
            lr.SetPosition(0, shootPoint.position);

            Ray ray = new Ray(shootPoint.position, shootPoint.forward);

            if (Physics.Raycast(ray,out RaycastHit hit, laserDistance, ~projectileLayer))
            { 
                lr.SetPosition(1, hit.point);
            }
            else
            {
                lr.SetPosition(1, shootPoint.position + shootPoint.forward * laserDistance);
            }
        }
    }

    private void KeyCheck()
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
            laserDistance = laserAimDistance;
        }

        if (Input.GetKeyUp(aimKey))
        {
            CameraManager.Instance.SwitchCamera(0);
            laserDistance = baseLaserDistance;
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
