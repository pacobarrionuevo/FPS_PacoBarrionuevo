using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] public PlayerWeaponSelector weaponSelector;

    void Update()
    {
        if (weaponSelector.activeWeapon != null)
        {
            weaponSelector.activeWeapon.Tick(Mouse.current.leftButton.isPressed);
        }
    }
}
