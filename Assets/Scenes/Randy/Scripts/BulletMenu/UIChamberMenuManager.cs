using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChamberMenuManager : MonoBehaviour
{
    public static UIChamberMenuManager instance;
    [SerializeField] Canvas canvas;
    [SerializeField] List<Image> chamberButtons;
    Revolver revolver;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        canvas.enabled = false;
    }

    void Update()
    {
        if (GameManager.instance && GameManager.instance.player && revolver == null)
            revolver = GameManager.instance.player.gameObject.GetComponent<PlayerController>().revolver;
    }

    public void SelectChamber(int c)
    {
        UISinSelectManager.instance.OpenSubMenu(c);
    }

    public void ExitSubMenu()
    {
        foreach(Bullet b in revolver.cylinder)
        {
            print("Bullet: " + b.type);
        }
    }

    public void SetSin(int c, Sin sin)
    {
        print("attempting to set sin");
        if(revolver == null)
        {
            print("revolver not found");
            return;
        }
        print("sin swapped");
        revolver.cylinder[c].type = SinManager.instance.GetSinObj(sin);
        // Change Button Color/Icon
    }

    public Vector3 GetChamberPosition(int c)
    {
        return chamberButtons[c].GetComponent<RectTransform>().position;
    }
    public void OpenMenu()
    {
        canvas.enabled = true;
    }

    public void CloseMenu()
    {
        // Close any submenus first
        UISinSelectManager.instance.CloseSubMenu();
        // Hide Canvas
        canvas.enabled = false;
    }
}
