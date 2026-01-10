using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Weapons/Shoot Configuration", order = 2)]
public class ShootConfigurationScriptableObject : ScriptableObject
{
    // How the gun shoots

    // The layer mask where the raycast will hit on
    public LayerMask HitMask;

    // The spread makes the bullet not to travel in a predictable straight line
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);

    // Delay between bullets
    public float fireRate = 0.25f;
}
