using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sin { NORMAL, PRIDE, GREED, WRATH, ENVY, LUST, GLUTTONY, SLOTH }
[System.Serializable]
public class Bullet
{
    [HideInInspector] public string name;
    public AbsSinClass type;
    public bool loaded;
}
