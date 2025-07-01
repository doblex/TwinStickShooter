using System;
using System.Collections;
using UnityEngine;
using utilities.Controllers;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float speed = 10f;
    [SerializeField] float lifetime = 5f;
    [SerializeField] int bounces = 3;

    Rigidbody rb;
    TrailRenderer tr;

    int currentBounces;

    public float Speed { get => speed; }

    private void Start()
    {
        currentBounces = 0;
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
    }

    public void ShowTr(bool show)
    {
        if (tr == null)
            tr = GetComponent<TrailRenderer>();
        if (tr != null)
            tr.enabled = show;
    }

    public void ResetStats()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();

        if (tr == null)
            tr = GetComponent<TrailRenderer>();

        currentBounces = 0;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StartCoroutine(DeactiveAfter(lifetime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            HandleCollision(collision);
            return;
        }

        if (currentBounces >= bounces)
        {
            tr.enabled = false;
            ObjectPooling.Instance.ReturnToPool(gameObject);
            return;
        }

        currentBounces++;

        rb.linearVelocity =  Vector3.Reflect(transform.forward * speed , collision.GetContact(0).normal);
        StopAllCoroutines();
        StartCoroutine(DeactiveAfter(lifetime));
    }

    private void HandleCollision(Collision collision)
    {
        collision.gameObject.GetComponent<HealthController>().DoDamage(damage);
        tr.enabled = false;
        ObjectPooling.Instance.ReturnToPool(gameObject);
    }

    private IEnumerator DeactiveAfter(float delay)
    { 
        yield return new WaitForSeconds(delay);
        ObjectPooling.Instance.ReturnToPool(gameObject);
    }
}
