using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] public PlayerWeaponSelector weaponSelector;
    [SerializeField] public WeaponScriptableObject weaponScriptableObject;

    void Update()
    {
        if (weaponSelector.activeWeapon != null)
        {
            weaponSelector.activeWeapon.Tick(Mouse.current.leftButton.isPressed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AmmoClip"))
        {
            weaponScriptableObject.currentAmmo = weaponScriptableObject.maxAmmo;
            AudioManager.Instance.PlayReloadAmmoSound();
            Destroy(other.gameObject);
        }
    }
}
