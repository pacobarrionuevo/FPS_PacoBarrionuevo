using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    [field: SerializeField] public Vector3 SpawnLocation { get; private set; }

    [field: SerializeField] public Vector3 SpawnVelocity { get; private set; }

    public delegate void CollisionEvent(Bullet bullet, Collision collision);
    public event CollisionEvent OnCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Spawn(Vector3 SpawnForce)
    {
        SpawnLocation = transform.position;
        transform.forward = SpawnForce.normalized;
        rb.AddForce(SpawnForce);
        SpawnVelocity = SpawnForce * Time.fixedDeltaTime / rb.mass;
        StartCoroutine(DelayedDisable(2));
    }

    private IEnumerator DelayedDisable(float Time)
    {
        yield return null;
        yield return new WaitForSeconds(Time);
        OnCollisionEnter(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(this, collision);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        OnCollision = null;
    }
}
