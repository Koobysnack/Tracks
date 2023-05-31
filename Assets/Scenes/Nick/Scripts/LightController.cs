using UnityEngine;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    [SerializeField]
    private List<Light> lights = new List<Light>();
    private int currentLightCount = 0;
    private const int maxLightCount = 4;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider component found on this GameObject. Please attach a BoxCollider for this script to function properly.");
            return;
        }

        foreach (LightTag lightTag in LightTag.AllLights)
        {
            foreach (Light light in lightTag.Lights)
            {
                if (boxCollider.bounds.Contains(light.transform.position))
                {
                    if (currentLightCount < maxLightCount)
                    {
                        light.enabled = false;
                        lights.Add(light);
                        currentLightCount++;
                    }
                    else
                    {
                        Debug.LogError($"Too many lights! Found more than {maxLightCount}. Please ensure no more than {maxLightCount} non-directional lights are within the collider on GameObject: {this.gameObject.name}");
                        return;
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }
    }
}
