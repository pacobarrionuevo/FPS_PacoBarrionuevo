using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject powerup;
    public static PrefabManager prefabManager;
    public bool isInfiniteAmmoActive = false;
    private ParticleSystem particleSystem_;

    public void Awake()
    {
        prefabManager = this;
        particleSystem_ = powerup.GetComponentInChildren<ParticleSystem>();
        DOTween.SetTweensCapacity(1000000, 1000000);
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
        particleSystem_.Play();
        weaponScriptableObject.currentAmmo = weaponScriptableObject.maxAmmo;
        AudioManager.Instance.PlayReloadAmmoSound();
        if (powerup != null)
        {
            DOTween.Kill(powerup.transform);
        }

        Destroy(other.gameObject);
    }

    public IEnumerator InfiniteAmmo(WeaponScriptableObject weaponScriptableObject, Collider other)
    {
        particleSystem_.Play();
        isInfiniteAmmoActive = true;
        AudioManager.Instance.PlayReloadAmmoSound();
        if (powerup != null)
        {
            DOTween.Kill(powerup.transform);
        }

        Destroy(other.gameObject);
        weaponScriptableObject.currentAmmo = 10000;
        weaponScriptableObject.shootConfig.fireRate /= 3;
        yield return new WaitForSeconds(10f);
        isInfiniteAmmoActive = false;
        weaponScriptableObject.currentAmmo = weaponScriptableObject.maxAmmo;
        weaponScriptableObject.shootConfig.fireRate *= 3;
    }

    public void RotateObject()
    {

        if (powerup == null) return;

        powerup.transform.DORotate(new Vector3(0f, 45, 0f), 1.5f, RotateMode.LocalAxisAdd).SetLoops(-1);

    }

    void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
