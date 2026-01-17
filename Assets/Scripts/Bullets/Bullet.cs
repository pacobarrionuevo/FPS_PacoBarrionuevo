using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [field: SerializeField]
    public Vector3 SpawnLocation
    {
        get;
        private set;
    }

    [SerializeField] private float delayedDisableTime = 2f;

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
        StartCoroutine(DelayedDisable(delayedDisableTime));
    }

    private IEnumerator DelayedDisable(float Time)
    {
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
