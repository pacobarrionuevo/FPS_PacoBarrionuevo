using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
public class WeaponScriptableObject : ScriptableObject
{
    public WeaponType type;
    public string name_;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;

    public ShootConfigurationScriptableObject shootConfig;
    public TrailConfigurationScriptableObject trailConfig;
    public DamageConfigScriptableObject damageConfig;

    [SerializeField] public int maxAmmo;
    [SerializeField] public int currentAmmo;
    private bool canShoot;

    private MonoBehaviour activeMonoBehaviour;
    private GameObject model;
    private float lastShootTime;

    private float initialClickTime;
    private float stopShootingTime;
    private bool lastFrameWantedToShoot;

    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;

    public void Awake()
    {
        currentAmmo = maxAmmo;
        canShoot = true;
    }

    public void Spawn(Transform parent, MonoBehaviour activeMonobehaviour)
    {
        this.activeMonoBehaviour = activeMonobehaviour;
        lastShootTime = 0;
        trailPool = new ObjectPool<TrailRenderer>(CreateTail);
        
        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);

        shootSystem = model.GetComponentInChildren<ParticleSystem>();
        Debug.Log(shootSystem);
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            canShoot = true;
        } 
        else
        {
            canShoot = false;
        }

        if (canShoot)
        {
            if (Time.time - shootConfig.fireRate - lastShootTime > Time.deltaTime)
            {
                float lastDuration = Mathf.Clamp(
                    0,
                    (stopShootingTime - initialClickTime),
                    shootConfig.maxSpreadTime
                );

                float lerpTime = (shootConfig.recoilRecoverySpeed - (Time.time - stopShootingTime))
                                    / shootConfig.recoilRecoverySpeed;

                initialClickTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
            }

            if (Time.time > shootConfig.fireRate + lastShootTime)
            {
                lastShootTime = Time.time;
                currentAmmo--;
                shootSystem.Play();

                Vector3 spreadAmount = shootConfig.GetSpread(Time.time - initialClickTime);
                model.transform.forward += model.transform.TransformDirection(spreadAmount);

                Vector3 shootDirection = model.transform.forward;

                if (Physics.Raycast(
                    shootSystem.transform.position,
                    shootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    shootConfig.HitMask))
                {
                    // if it hits an enemy
                    activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));
                }
                else
                {
                    // if it misses an enemy
                    activeMonoBehaviour.StartCoroutine(PlayTrail(shootSystem.transform.position,
                        shootSystem.transform.position + (shootDirection * trailConfig.missDistance),
                        new RaycastHit()));
                }
            }
        }
        
    }

    public void Tick(bool WantsToShoot)
    {
        model.transform.localRotation = Quaternion.Lerp(
            model.transform.localRotation,
            Quaternion.Euler(spawnRotation),
            Time.deltaTime * shootConfig.recoilRecoverySpeed
        );

        if (WantsToShoot)
        {
            lastFrameWantedToShoot = true;
            Shoot();
        }
        else if (!WantsToShoot && lastFrameWantedToShoot) 
        {
            stopShootingTime = Time.time;
            lastFrameWantedToShoot = false;
        }
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        // avoid position carry - over from last frame if used
        yield return null;

        instance.emitting = true;
        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                startPoint,
                endPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );

            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damageConfig.GetDamage(distance));
            }
        }

        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;

        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
    }

    private TrailRenderer CreateTail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
