using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerWeaponSelector : MonoBehaviour
{
    [SerializeField] public WeaponType weapon;
    [SerializeField] public Transform weaponParent;
    [SerializeField] public List<WeaponScriptableObject> weapons;

    [Space]
    [Header("Runtime filled")]
    public WeaponScriptableObject activeWeapon;

    private void Start()
    {
        WeaponScriptableObject wp = weapons.Find(wp => wp.type == weapon);

        if (wp == null)
        {
            Debug.LogError($"No WeaponScriptableObject found for WeaponType: {wp}");
            return;
        }

        activeWeapon = wp;
        wp.Spawn(weaponParent, this);
    }
}
