using UnityEngine;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    public string room;  // The room that this controller manages
    [SerializeField]
    private List<Light> lights = new List<Light>();

    private void Start()
    {
        foreach (LightTag lightTag in FindObjectsOfType<LightTag>())
        {
            if (lightTag.room == room)
            {
                lights.Add(lightTag.Light);
            }
        }

        if (lights.Count > 4)
        {
            Debug.LogError($"Too many lights in {room} on GameObject: {this.gameObject.name}. Please ensure no more than 4 non-directional lights are added.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
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
