using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent] 
public class PlayerWeaponSelector : MonoBehaviour 
{ 
    [Header("References")]
    [SerializeField] public Transform weaponParent; 
    [Header("All available weapons")]
    [SerializeField] public List<WeaponScriptableObject> weapons; 
    [Header("Runtime filled")] 
    public WeaponScriptableObject activeWeapon; 
    private PlayerWeaponSwitcher weaponSwitcher; 

    private void Awake() {
        weaponSwitcher = GetComponentInChildren<PlayerWeaponSwitcher>();
        if (weaponSwitcher == null) {
            Debug.LogError("PlayerWeaponSwitcher not found on object."); 
        } 
    } 
    private void Start() {
        UpdateActiveWeapon();
    } 
    public void UpdateActiveWeapon() 
    { 
        int index = weaponSwitcher.GetSelectedWeaponIndex();
        if (index < 0 || index >= weapons.Count)
        {
            Debug.LogError("Invalid weapon index.");
            return;
        }
        activeWeapon = weapons[index]; 
    } 
    public WeaponScriptableObject GetActiveWeapon() { return activeWeapon; } }