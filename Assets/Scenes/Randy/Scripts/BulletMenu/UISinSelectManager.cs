using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISinSelectManager : MonoBehaviour
{
    public static UISinSelectManager instance;
    [SerializeField] List<Image> buttons;
    public bool isOpen { get; private set; }
    int selectedChamber;
    RectTransform rt;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        rt = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        foreach(Image b in buttons)
        {
            b.alphaHitTestMinimumThreshold = 0.5f;
        }
    }

    public void OnClick(int selection)
    {
        print("chamber " + selectedChamber + ": " + (Sin)selection);
        UIChamberMenuManager.instance.SetSin(selectedChamber, (Sin)selection);
        CloseMenu();
    }

    public void OpenMenu(int chamber)
    {
        if (isOpen && chamber == selectedChamber)
        {
            CloseMenu();
            return;
        }
        rt.position = UIChamberMenuManager.instance.GetChamberPosition(chamber);
        isOpen = true;
        selectedChamber = chamber;
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }    
}
