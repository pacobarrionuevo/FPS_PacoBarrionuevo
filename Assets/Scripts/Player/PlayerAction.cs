using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] public PlayerWeaponSelector weaponSelector;

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && weaponSelector.activeWeapon != null)
        {
            weaponSelector.activeWeapon.Shoot();
        }
    }
}
