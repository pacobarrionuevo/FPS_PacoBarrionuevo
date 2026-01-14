using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ammoText;
    [SerializeField] public WeaponScriptableObject weaponScriptable;

    private void Update()
    {
        if (weaponScriptable.currentAmmo > 0)
        {
            ammoText.text = $"{weaponScriptable.currentAmmo}";
        }
        else
        {
            ammoText.text = "RELOAD!";
        }
        
    }
}
