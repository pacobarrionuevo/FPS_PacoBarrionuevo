using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class SpawnParticleSystemOnDeath : MonoBehaviour
{
    [SerializeField] public GameObject gameObject;
    [SerializeField] private ParticleSystem deathSystem;
    public IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>(); 
    }

    private void OnEnable()
    {
        damageable.OnDeath += DamageableOnDeath;
    }

    private void DamageableOnDeath(Vector3 position)
    {
        Instantiate(deathSystem, position, Quaternion.identity);
        Destroy(gameObject);
    }
}
