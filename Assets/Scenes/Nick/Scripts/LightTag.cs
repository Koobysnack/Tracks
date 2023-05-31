using UnityEngine;

public class LightTag : MonoBehaviour
{
    public string room;  // The room that this light belongs to
    public Light Light { get; private set; }

    private void Awake()
    {
        Light = GetComponent<Light>();
        Light.enabled = false;
    }
}
