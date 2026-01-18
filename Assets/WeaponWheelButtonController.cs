using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelButtonController : MonoBehaviour
{
    public int Id;
    private Animator anim;
    public string itemName;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
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
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
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
