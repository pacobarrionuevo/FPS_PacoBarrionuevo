using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] public PlayerWeaponSelector weaponSelector;
    [SerializeField] public List<WeaponScriptableObject> weapons;

    void Update()
    {
        if (weaponSelector.activeWeapon != null)
        {
            weaponSelector.activeWeapon.Tick(Mouse.current.leftButton.isPressed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AmmoClip") && !PrefabManager.prefabManager.isInfiniteAmmoActive)
        {
            PrefabManager.prefabManager.ReloadAmmo(weapons[WeaponWheelButtonController.wwbc.Id], other);
        }

        if (other.CompareTag("InfiniteAmmo"))
        {
            StartCoroutine(PrefabManager.prefabManager.InfiniteAmmo(weapons[WeaponWheelButtonController.wwbc.Id], other));
        }
    }
}
