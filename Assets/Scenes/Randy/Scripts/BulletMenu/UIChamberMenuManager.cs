using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChamberMenuManager : MonoBehaviour
{
    public static UIChamberMenuManager instance;
    [SerializeField] List<Image> chamberButtons;
    public List<Bullet> chambers;

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
    }

    public void SelectChamber(int c)
    {
        UISinSelectManager.instance.OpenMenu(c);
    }

    public void ExitMenu()
    {
        foreach(Bullet b in chambers)
        {
            print("Bullet: " + b.type);
        }
    }

    public void SetSin(int c, Sin sin)
    {
        //print("set!");
        //chambers[c].type = sin;
        // Change Button Color/Icon
    }
}
