using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISinSelectManager : MonoBehaviour
{
    public static UISinSelectManager instance;
    [SerializeField] List<GameObject> buttonSets;
    public bool isOpen { get; private set; }
    int selectedChamber = -1;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    public void SelectSin(Sin selection)
    {
        print("chamber " + selectedChamber + ": " + selection);
        UIChamberMenuManager.instance.SetSin(selectedChamber, selection);
        CloseMenu();
    }

    public void OpenMenu(int chamber)
    {
        if (isOpen && chamber == selectedChamber)
        {
            CloseMenu();
            return;
        }
        // Close previous menu
        if(isOpen)
            CloseMenu();
        isOpen = true;
        selectedChamber = chamber;
        buttonSets[selectedChamber].SetActive(true);
    }

    public void CloseMenu()
    {
        isOpen = false;
        buttonSets[selectedChamber].SetActive(false);
    }    
}
