using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinController : ScriptableObject
{
    [SerializeField]
    
    private bool[] sinEnabled;


    void Awake()
    {
        sinEnabled = new bool[10];
    }

    public void SetSin()
    {

    }

}
