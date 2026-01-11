using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Weapons/Shoot Configuration", order = 2)]
public class ShootConfigurationScriptableObject : ScriptableObject
{
    // How the gun shoots

    // The layer mask where the raycast will hit on
    public LayerMask HitMask;

    // Delay between bullets
    public float fireRate = 0.25f;

    public float recoilRecoverySpeed = 1f;
    public float maxSpreadTime = 1f;

    public BulletSpreadType bulletSpreadType = BulletSpreadType.Simple;

    // The spread makes the bullet not to travel in a predictable straight line
    [Header("Simple Spread")]
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);

    public Vector3 GetSpread(float shootTime = 0)
    {
        Vector3 spread = Vector3.zero;

        if (bulletSpreadType == BulletSpreadType.Simple)
        {
            spread = Vector3.Lerp(
                Vector3.zero,
                new Vector3(Random.Range(-Spread.x, Spread.x),
                     Random.Range(-Spread.y, Spread.y),
                     Random.Range(-Spread.z, Spread.z)), 
                Mathf.Clamp01(shootTime/maxSpreadTime));            
        }

        return spread;
    }
}
