using UnityEngine;
using System.Collections.Generic;

public class LightTag : MonoBehaviour
{
    public static List<LightTag> AllLights = new List<LightTag>();
    public List<Light> Lights { get; private set; }

    void Awake()
    {
        Lights = new List<Light>(GetComponentsInChildren<Light>());
        
        foreach (var light in Lights)
        {
            if(light.type != LightType.Directional)
            {
                light.enabled = false;
                AllLights.Add(this);
            }
        }
    }

    void OnDestroy()
    {
        AllLights.Remove(this);
    }
}
