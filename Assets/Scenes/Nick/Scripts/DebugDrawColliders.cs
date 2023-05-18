using UnityEngine;

public class DebugDrawColliders : MonoBehaviour
{
    bool drawColliders = false;

    void Update()
    {
        // Toggle drawColliders when the user presses the F1 key
        if (Input.GetKeyDown(KeyCode.F1))
        {
            drawColliders = !drawColliders;
        }
    }

    void OnDrawGizmos()
    {
        // Only draw the colliders if drawColliders is true
        if (drawColliders)
        {
            foreach (BoxCollider boxCollider in GetComponentsInChildren<BoxCollider>())
            {
                Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
            }

            foreach (SphereCollider sphereCollider in GetComponentsInChildren<SphereCollider>())
            {
                Gizmos.DrawWireSphere(sphereCollider.bounds.center, sphereCollider.radius);
            }

            foreach (MeshCollider meshCollider in GetComponentsInChildren<MeshCollider>())
            {
                DrawWireframe(meshCollider.sharedMesh, meshCollider.transform.position, meshCollider.transform.rotation, meshCollider.transform.localScale);
            }
        }
    }

    void DrawWireframe(Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 start = position + rotation * Vector3.Scale(vertices[triangles[i]], scale);
            Vector3 middle = position + rotation * Vector3.Scale(vertices[triangles[i + 1]], scale);
            Vector3 end = position + rotation * Vector3.Scale(vertices[triangles[i + 2]], scale);

            Gizmos.DrawLine(start, middle);
            Gizmos.DrawLine(middle, end);
            Gizmos.DrawLine(end, start);
        }
    }


}
