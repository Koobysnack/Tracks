using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SinManager : MonoBehaviour
{
    // public enum Sin { NORMAL, PRIDE, GREED, WRATH, ENVY, LUST, GLUTTONY, SLOTH }
    public static SinManager instance;
    BiDict<Sin, AbsSinClass> sins = new BiDict<Sin, AbsSinClass>();
    [SerializeField] List<Sin> unlockedSins;
    [SerializeField, Foldout("SinObjs")] AbsSinClass noSin;
    [SerializeField, Foldout("SinObjs")] AbsSinClass prideSin;
    [SerializeField, Foldout("SinObjs")] AbsSinClass greedSin;
    [SerializeField, Foldout("SinObjs")] AbsSinClass wrathSin;
    [SerializeField, Foldout("SinObjs")] AbsSinClass envySin;
    [SerializeField, Foldout("SinObjs")] AbsSinClass lustSin;
    [SerializeField, Foldout("SinObjs")] AbsSinClass gluttonySin;
    [SerializeField, Foldout("SinObjs")] AbsSinClass slothSin;

    [Foldout("SinColors")] public Color normalBulletColor;
    [Foldout("SinColors")] public Color prideBulletColor;
    [Foldout("SinColors")] public Color greedBulletColor;
    [Foldout("SinColors")] public Color wrathBulletColor;
    [Foldout("SinColors")] public Color envyBulletColor;
    [Foldout("SinColors")] public Color lustBulletColor;
    [Foldout("SinColors")] public Color gluttonyBulletColor;
    [Foldout("SinColors")] public Color slothBulletColor;

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

    public void UnlockSin(Sin s)
    {
        unlockedSins.Add(s);
    }

    public Color GetSinColor(Sin sin)
    {
        switch (sin)
        {
            case Sin.NORMAL:
                return normalBulletColor;
            case Sin.PRIDE:
                return prideBulletColor;
            case Sin.GREED:
                return greedBulletColor;
            case Sin.WRATH:
                return wrathBulletColor;
            case Sin.ENVY:
                return envyBulletColor;
            case Sin.LUST:
                return lustBulletColor;
            case Sin.GLUTTONY:
                return gluttonyBulletColor;
            case Sin.SLOTH:
                return slothBulletColor;
            default:
                return Color.magenta;
        }
    }

    public Color GetSinColor(AbsSinClass sin)
    {
        return GetSinColor(GetSinEnum(sin));
    }
}
