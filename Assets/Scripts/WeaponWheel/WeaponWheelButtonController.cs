using UnityEngine;
using UnityEngine.UI;
public class WeaponWheelButtonController : MonoBehaviour 
{
    public static WeaponWheelButtonController wwbc;
    public int Id; 
    private Animator anim;
    public string itemName;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;
    public bool isHovered = false;
    void Start() 
    { 
        anim = GetComponent<Animator>();
        wwbc = this;
    } 
    void Update() 
    {
        if (selected) 
        { 
            selectedItem.sprite = icon;
        }
    }
    public void HoverEnter() 
    {
        anim.SetBool("Hover", true);
        AudioManager.Instance.PlayHoverSound();
        isHovered = true;
    }
    public void HoverExit() 
    {
        anim.SetBool("Hover", false);
        isHovered = false;
    }
    public void Selected() 
    {
        selected = true;
        WeaponWheelController.weaponId = Id; 
    } 
    public void Deselected() 
    {
        selected = false;
        WeaponWheelController.weaponId = -1; 
    }
}