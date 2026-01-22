using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] public TextMeshProUGUI controlsText;

    private PlayerWeaponSwitcher weaponSwitcher;

    private bool ControlsPanel = false;

    private void Start()
    {
        weaponSwitcher = FindFirstObjectByType<PlayerWeaponSwitcher>();

        if (weaponSwitcher == null)
            Debug.LogError("PlayerWeaponSwitcher not found in scene!");
    }

    private void Update()
    {
        UpdateAmmoHUD();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ControlsPanel = !ControlsPanel;
        }

        if (ControlsPanel)
        {
            controlsText.gameObject.SetActive(true);
        }
        else
        {
            controlsText.gameObject.SetActive(false);

        }
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
