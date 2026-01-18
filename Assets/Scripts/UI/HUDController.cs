using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ammoText;
    [SerializeField] public WeaponScriptableObject[] weaponScriptables;
    [SerializeField] public int position = 0;

    private void Update()
    {
        ShowAmmo(position);        
    }

    public void ShowAmmo(int position)
    {
        if (weaponScriptables[position].currentAmmo > 0)
        {
            ammoText.text = $"{weaponScriptables[position].currentAmmo}";
        }
        else
        {
            ammoText.text = "RELOAD!";
        }
    }
}
