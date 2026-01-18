using DG.Tweening;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject powerup;
    public static PrefabManager prefabManager;
    private Tweener tween;

    public void Awake()
    {
        prefabManager = this;
    }

    public void Update()
    {
        if (powerup != null)
        {
            RotateObject();
        } 
    }

    public void ReloadAmmo(WeaponScriptableObject weaponScriptableObject, Collider other)
    {
        weaponScriptableObject.currentAmmo = weaponScriptableObject.maxAmmo;
        AudioManager.Instance.PlayReloadAmmoSound();
        if (powerup != null)
        {
            DOTween.Kill(powerup.transform);
        }

        Destroy(other.gameObject);
    }

    public void RotateObject()
    {
        DOTween.SetTweensCapacity(10000, 10000);

        if (powerup == null) return;

        if (tween == null || !tween.IsPlaying())
        {
            tween = powerup.transform.DORotate(new Vector3(0f, 45, 0f), 1.5f, RotateMode.LocalAxisAdd)
                       .SetLoops(-1);
        }
    }

    void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
