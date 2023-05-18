using UnityEngine;

public class HideMeshRenderer : MonoBehaviour
{
    void Start()
    {
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }
}
