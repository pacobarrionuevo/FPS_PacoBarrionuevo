using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Weapons/Weapon Trail Configuration", order = 4)]
public class TrailConfigurationScriptableObject : ScriptableObject
{
    // To show the bullet trail and where it lands
    // Similar to the trail renderer component

    public Material material;
    public AnimationCurve widthCurve;
    public float duration = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient color;

    // How far the bullet gets seen
    public float missDistance = 100f;

    // The speed that sets how quickly the bullet travels from the shooting point to the missDistance
    public float simulationSpeed = 100f;
    
}
