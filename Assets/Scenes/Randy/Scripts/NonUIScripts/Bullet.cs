using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Sin { NORMAL, PRIDE, GREED, WRATH, ENVY, LUST, GLUTTONY, SLOTH }
[System.Serializable]
public class Bullet
{
    [HideInInspector] public string name;
    public Sin type;
    public bool loaded;
}
