using UnityEngine;
using UnityEngine.VFX;

public class BulletEffectManager : MonoBehaviour
{
    public GameObject bulletShotEffect;
    public GameObject bulletTrailEffect;
    public GameObject hitEffectPrefab;
    public GameObject muzzleFlash;

    public void TriggerShotEffect()
    {
        bulletShotEffect.GetComponent<ParticleSystem>().Play();
        muzzleFlash.GetComponent<VisualEffect>().Play();
    }
  
    public void CreateBulletTrail(Vector3 startPosition, Vector3 endPosition)
    {
        // Calculate the middle position
        Vector3 middlePosition = (startPosition + endPosition) / 2.0f;

        // Calculate the distance between start and end positions
        float distance = Vector3.Distance(startPosition, endPosition);

        // Calculate the direction from start to end
        Vector3 direction = (endPosition - startPosition).normalized;

        // Instantiate the bullet trail effect at the middle position
        if(bulletTrailEffect != null) {
            GameObject bulletTrail = Instantiate(bulletTrailEffect, middlePosition, Quaternion.LookRotation(direction));
            // Stretch the bullet trail to span the entire distance
            ParticleSystem.ShapeModule shape = bulletTrail.GetComponent<ParticleSystem>().shape;
            shape.radius = distance;
            Debug.Log(shape.radius);
            // Destroy the bullet trail after some time
            Destroy(bulletTrail, 1.0f);
        }
    }
    public void CreateHitEffect(Vector3 position, Vector3 normal)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);
        GameObject hitEffect = Instantiate(hitEffectPrefab, position, rotation);
    }

}
