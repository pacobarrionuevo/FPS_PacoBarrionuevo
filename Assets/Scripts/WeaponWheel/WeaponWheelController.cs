using UnityEngine;
using UnityEngine.UI;
public class WeaponWheelController : MonoBehaviour 
{ 
    public Animator anim;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponId;
    [SerializeField] private PlayerWeaponSwitcher switcher;

    public void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            OpenWheel();
        }
        else
        {
            CloseWheel();
        }

        switch (weaponId)
        {
            case 0:
                switcher.SelectWeaponFromWheel(0);
                break;

            case 1:
                switcher.SelectWeaponFromWheel(1);
                break;

            case 2:
                switcher.SelectWeaponFromWheel(2);
                break;

            case 3:
                switcher.SelectWeaponFromWheel(3);
                break;

            default:
                selectedItem.sprite = noImage;
                break;
        }
    }

    private void OpenWheel()
    {
        anim.SetBool("OpenWeaponWheel", true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseWheel()
    {
        anim.SetBool("OpenWeaponWheel", false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}