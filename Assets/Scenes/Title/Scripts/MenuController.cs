using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject titleScreenParent, optionScreenParent;
    
    public void OpenOptions()
    {
        titleScreenParent.SetActive(false);
        optionScreenParent.SetActive(true);
    }

    public void CloseOptions()
    {
        optionScreenParent.SetActive(false);
        titleScreenParent.SetActive(true);
    }
}
