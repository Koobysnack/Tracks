using UnityEngine;

public class SwingingObject : MonoBehaviour
{
    public float speed = 1.0f;
    public float maxRotation = 45.0f;

    void Update()
    {
        // Calculate the new rotation angle
        float angle = maxRotation * Mathf.Sin(Time.time * speed);
        // Apply the rotation around the z axis
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
