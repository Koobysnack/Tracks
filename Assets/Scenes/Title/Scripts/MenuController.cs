using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject titleScreenParent,controlsParent, optionScreenParent, creditsParent;
    
    public void OpenControls()
    {
        titleScreenParent.SetActive(false);
        controlsParent.SetActive(true);
    }

    public void CloseControls()
    {
        titleScreenParent.SetActive(true);
        controlsParent.SetActive(false);
    }

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

    public void OpenCredits()
    {
        creditsParent.SetActive(true);
        optionScreenParent.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsParent.SetActive(false);
        optionScreenParent.SetActive(true);
    }

}
