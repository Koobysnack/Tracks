using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinManager : MonoBehaviour
{
    // public enum Sin { NORMAL, PRIDE, GREED, WRATH, ENVY, LUST, GLUTTONY, SLOTH }
    public static SinManager instance;
    BiDict<Sin, AbsSinClass> sins = new BiDict<Sin, AbsSinClass>();
    [SerializeField] AbsSinClass noSin;
    [SerializeField] AbsSinClass prideSin;
    [SerializeField] AbsSinClass greedSin;
    [SerializeField] AbsSinClass wrathSin;
    [SerializeField] AbsSinClass envySin;
    [SerializeField] AbsSinClass lustSin;
    [SerializeField] AbsSinClass gluttonySin;
    [SerializeField] AbsSinClass slothSin;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        InitDict();   
    }

    void InitDict()
    {
        sins.Add(Sin.NORMAL, noSin == null ? null : noSin);
        sins.Add(Sin.PRIDE, prideSin == null ? null : prideSin);
        sins.Add(Sin.GREED, greedSin == null ? null : greedSin);
        sins.Add(Sin.WRATH, wrathSin == null ? null : wrathSin);
        sins.Add(Sin.ENVY, envySin == null ? null : envySin);
        sins.Add(Sin.LUST, lustSin == null ? null : lustSin);
        sins.Add(Sin.GLUTTONY, gluttonySin == null ? null : gluttonySin);
        sins.Add(Sin.SLOTH, slothSin == null ? null : slothSin);
    }

    public Sin GetSinEnum(AbsSinClass sin)
    {
        Sin s;
        sins.TryGetValue(sin, out s);
        return s;
    }

    public AbsSinClass GetSinObj(Sin sin)
    {
        AbsSinClass s;
        sins.TryGetValue(sin, out s);
        return s;
    }
}
