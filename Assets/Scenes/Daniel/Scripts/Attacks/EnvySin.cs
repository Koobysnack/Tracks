using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvySin : AbsSinClass 
{

    [SerializeField]
    //private EntityController EntityRef;
    private LayerMask lMask;

    void OnAwake()
    {
        lMask = LayerMask.GetMask("Enemy,Ground");
    }
    public override void SinFire(Transform shotOrigin )
    {
        RaycastHit hit;
        
    
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(shotOrigin.position, shotOrigin.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(shotOrigin.position, shotOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red,10000);
            Debug.Log("Did Hit");

            //  RaycastHit pierceHit;

            // print(hit);
           
                EnvyPierce(hit, shotOrigin.TransformDirection(Vector3.forward),shotOrigin);
            
        }
        else
        {
            Debug.DrawRay(shotOrigin.position,shotOrigin.TransformDirection(Vector3.forward) * 1000, Color.white,1000000);
            Debug.Log("Did not Hit");
        }

    }
    //recursively call this function each time a pierce occurs
    private void EnvyPierce(RaycastHit obj, Vector3 fireAngle,Transform shotOrigin)
    {
     //   RaycastHit hit;
        RaycastHit backHit;
        RaycastHit nextObj;


        
        Ray backRay = new Ray(obj.point + fireAngle * 6, -1 * fireAngle * 6);
     
            obj.collider.Raycast(backRay, out backHit, 100);
       // Debug.Log(backHit.transform.gameObject.name);
        if (backHit.collider!=null && backHit.transform.parent)
        {
            EntityController ShotEntity;
            ShotEntity = backHit.transform.parent.GetComponent<EntityController>();
            if (ShotEntity)
            {
                ShotEntity.Damage(10, shotOrigin);

            }

        }
        //then get next  object, 
        Debug.Log("stinky");
            if (Physics.Raycast(backHit.point, fireAngle, out nextObj, 1000,lMask)) {
                Debug.DrawRay(backHit.point, fireAngle*6, Color.green,10000);

            EnvyPierce(nextObj, fireAngle, shotOrigin);
                
            }
            else
            {
                return;
            }

            //pass obj into envy again

        
        
    } 
}
