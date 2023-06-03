using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitScanRaycast : MonoBehaviour
{



   
    [SerializeField]
    protected EntityController EntityRef;
    public BulletEffectManager bulletEffectManager;
    public GameObject gunBarrel;
    public int pierceAmount;
    public int pierceMax;

    void Start()
    {
    
    }



    #region BaseRays
    public void RegRayCaster()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
         //   Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white,1000f);
           // Debug.Log("Did not Hit");
        }

    }

    public void PierceRayCaster()
    {
        RaycastHit hit;
        RaycastHit backHit;
        RaycastHit hit2;
        Ray pierceRay;
        EntityController ShotEntity;
     //       if (pierceAmount >= pierceMax)
       // {
         //   return;
    //    }
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            bulletEffectManager.CreateBulletTrail(gunBarrel.transform.position, hit.point);
            bulletEffectManager.CreateHitEffect(hit.point, hit.normal);
            bulletEffectManager.TriggerShotEffect();
          //  Debug.Log("Did Hit");
          //  RaycastHit pierceHit;
            
            pierceRay = new Ray(hit.point + transform.TransformDirection(Vector3.forward) * 6, -1*transform.TransformDirection(Vector3.forward)*6);
            Debug.DrawRay(hit.point + transform.TransformDirection(Vector3.forward)*6, -1 * transform.TransformDirection(Vector3.forward)*6, Color.red);

            hit.collider.Raycast(pierceRay,out backHit,1000);
            if (hit.transform.parent)
            {

                ShotEntity = hit.transform.parent.GetComponent<EntityController>();
                ShotEntity = ShotEntity == null ? hit.transform.GetComponent<EntityController>() : ShotEntity;
                if (ShotEntity)
                {
                    
                    ShotEntity.Damage(EntityRef.damage, transform);

                }
            }

            if (backHit.collider==null)
            {
               
                
                return;
            }

            if (Physics.Raycast(backHit.point, transform.TransformDirection(Vector3.forward), out hit2, Mathf.Infinity))
            {
                if (backHit.transform == null || backHit.transform.parent == null)
                    return;
                Debug.DrawRay(backHit.point, transform.TransformDirection(Vector3.up), Color.blue);
               
                if (backHit.transform.parent.TryGetComponent<EntityController>(out ShotEntity))
                {
                    ShotEntity.Damage(EntityRef.damage,transform);

                }
            }
        }
        else
        {
         //   Debug.DrawRay(gunBarrel.transform.position, gunBarrel.transform.TransformDirection(Vector3.left) * 1000, Color.white,100f);
          // Debug.Log(gunBarrel.transform.position);
          //  Debug.Log("Did not Hit");
       //    bulletEffectManager.CreateBulletTrail(gunBarrel.transform.position,transform.TransformPoint(gunBarrel.transform.localPosition+ gunBarrel.transform.TransformDirection(Vector3.forward) * 1000));
         //  Debug.Log(gunBarrel.transform.position + new Vector3(10,10,10));
    //     bulletEffectManager.TriggerShotEffect();
        }

    }
    #endregion

  

}
