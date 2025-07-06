using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Rigidbody rb;

    bool lockedInput = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<EntityPlayer>().lockInput += Locked;
    }

    private void Locked(bool locked)
    {
        lockedInput = locked;
    }


    private void FixedUpdate()
    {
        if (lockedInput) return;

        float Hz = Input.GetAxis("Horizontal");
        float Vt = Input.GetAxis("Vertical");

        if (Hz != 0 || Vt != 0)
        {
            Vector3 direction = new Vector3(Hz, 0, Vt).normalized;

            rb.linearVelocity = direction * speed;
        }
    }
    private void Update()
    {
        if (lockedInput) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Keep the y position of the player

            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }

}
