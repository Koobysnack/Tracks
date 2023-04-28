using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvySin : AbsSinClass 
{

    [SerializeField]
    private EntityController EntityRef;

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


        //get same object but back face
        Ray backRay = new Ray(obj.point + fireAngle * 6, -1 * fireAngle * 6);
        if (obj.transform.gameObject.tag == "Wall"  )
        {
           
            Debug.Log("yeouch");
        }
        else {
            obj.collider.Raycast(backRay, out backHit, 100);

            //then get next  object, 

            if (Physics.Raycast(backHit.point, fireAngle, out nextObj, 1000)) {
                Debug.DrawRay(backHit.point, fireAngle*2, Color.green,10000);
                
                EntityController ShotEntity;
                ShotEntity = backHit.transform.gameObject.GetComponent<EntityController>();
                if (ShotEntity )
                {
                    ShotEntity.Damage(EntityRef.damage, shotOrigin);
                   
                }
                EnvyPierce(nextObj, fireAngle,shotOrigin);

            }
            else
            {
                return;
            }

            //pass obj into envy again

        }
        
    } 
}
