using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitScanRaycast : MonoBehaviour
{


    //consturctor
    [SerializeField]
    private EntityController EntityRef;

    public void RegRayCaster()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white,1000f);
            Debug.Log("Did not Hit");
        }

    }
    public void PierceRayCaster()
    {
        RaycastHit hit;
        RaycastHit backHit;
        RaycastHit hit2;
        Ray pierceRay;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
          //  RaycastHit pierceHit;
            pierceRay = new Ray(hit.point + transform.TransformDirection(Vector3.forward) * 6, -1*transform.TransformDirection(Vector3.forward)*6);
            Debug.DrawRay(hit.point + transform.TransformDirection(Vector3.forward)*6, -1 * transform.TransformDirection(Vector3.forward)*6, Color.red);
            hit.collider.Raycast(pierceRay,out backHit,1000);

            if (Physics.Raycast(backHit.point, transform.TransformDirection(Vector3.forward), out hit2, Mathf.Infinity))
            {

                Debug.DrawRay(backHit.point, transform.TransformDirection(Vector3.up), Color.blue);
                EntityController ShotEntity;
                ShotEntity = hit2.transform.gameObject.GetComponent<EntityController>();
                if (ShotEntity)
                {
                    ShotEntity.Damage(EntityRef.damage,transform);

                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

    }

}