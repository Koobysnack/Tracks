using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISinMenu : MonoBehaviour
{
    UISinSelectManager uiSSM;
    [SerializeField] List<Image> buttons;

    void Start()
    {
        foreach (Image b in buttons)
        {
            b.alphaHitTestMinimumThreshold = 0.5f;
        }
        gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(UISinSelectManager.instance != null && uiSSM == null)
            uiSSM = UISinSelectManager.instance;
    }

    public void SelectSin(int sin)
    {
        uiSSM.SelectSin((Sin)sin);
    }

    void OnEnable()
    {
        // Check available sins on enable
    }
}
