using UnityEngine;
public class PlayerWeaponSwitcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponScriptableObject[] weaponDefinitions; 
    [SerializeField] private Transform weaponParent; 
    [Header("Keys")]
    [SerializeField] private KeyCode[] keys; 
    [Header("Settings")]
    [SerializeField] private float switchTime = 0.25f; 
    private PlayerWeaponSelector selector; private Transform[] weapons; 
    private int selectedWeapon; private float timeSinceLastSwitch; 

    private void Start() { 
        selector = GetComponentInParent<PlayerWeaponSelector>(); 
        SpawnAllWeapons(); 
        Select(0); 
        timeSinceLastSwitch = 0f; 
    }
    private void Update() 
    { 
        for (int i = 0; i < keys.Length; i++) 
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime) 
            {
                Select(i); 
            } 
        } 
        timeSinceLastSwitch += Time.deltaTime; 
    }

    private void SpawnAllWeapons() 
    { 
        weapons = new Transform[weaponDefinitions.Length]; 

        for (int i = 0; i < weaponDefinitions.Length; i++) 
        { 
            weaponDefinitions[i].Spawn(weaponParent, this); 
            weapons[i] = weaponParent.GetChild(i); 
            weapons[i].gameObject.SetActive(false); 
        } 
        
        if (keys == null || keys.Length == 0) 
        { 
            keys = new KeyCode[weapons.Length]; 
            for (int i = 0; i < weapons.Length; i++) keys[i] = KeyCode.Alpha1 + i; 
        } 
    }

    private void Select(int weaponIndex)
    { 
        selectedWeapon = weaponIndex; 
        for (int i = 0; i < weapons.Length; i++)
        { 
            weapons[i].gameObject.SetActive(i == weaponIndex); 
        }

        selector?.UpdateActiveWeapon(); timeSinceLastSwitch = 0f; 
    }

    public WeaponScriptableObject GetActiveWeapon() { return weaponDefinitions[selectedWeapon]; }
    public int GetSelectedWeaponIndex() { return selectedWeapon; }

    public void SelectWeaponFromWheel(int index)
    {
        Select(index);
    }
}