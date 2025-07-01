using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float Hz = Input.GetAxis("Horizontal");
        float Vt = Input.GetAxis("Vertical");

        if (Hz != 0 || Vt != 0)
        {
            Vector3 direction = new Vector3(Hz, 0, Vt).normalized;

            rb.linearVelocity = direction * speed;

            //Vector3 targetVelocity = direction * speed;

            //transform.position += targetVelocity;
        }
    }
}
