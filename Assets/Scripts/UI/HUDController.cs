using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private PlayerWeaponSwitcher weaponSwitcher;

    private void Start()
    {
        weaponSwitcher = FindFirstObjectByType<PlayerWeaponSwitcher>();

        if (weaponSwitcher == null)
            Debug.LogError("PlayerWeaponSwitcher not found in scene!");
    }

    private void Update()
    {
        UpdateAmmoHUD();
    }

    private void UpdateAmmoHUD()
    {
        WeaponScriptableObject weapon = weaponSwitcher.GetActiveWeapon();

        if (weapon == null)
        {
            ammoText.text = "";
            return;
        }

        if (weapon.currentAmmo > 0)
        {
            ammoText.text = weapon.currentAmmo.ToString();
        }
        else
        {
            ammoText.text = "RELOAD!";
        }
    }
}
