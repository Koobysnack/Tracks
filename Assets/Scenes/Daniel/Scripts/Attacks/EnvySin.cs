using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvySin : AbsSinClass 
{

    [SerializeField]
    //private EntityController EntityRef;
    private LayerMask lMask;

    void OnEnable()
    {
        lMask = LayerMask.GetMask("Enemy,Ground");
    }

    
    public override void SinFire(Transform shotOrigin )
    {
          
        RaycastHit hit;
        lMask = LayerMask.GetMask("Enemy,Ground");


        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(shotOrigin.position, shotOrigin.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(shotOrigin.position, shotOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red,1000);
            Debug.Log("Did Hit");

            //  RaycastHit pierceHit;

            // print(hit);
           
                EnvyPierce(hit, shotOrigin.TransformDirection(Vector3.forward),shotOrigin);
            
        }
        else
        {
            Debug.DrawRay(shotOrigin.position,shotOrigin.TransformDirection(Vector3.forward) * 1000, Color.white,1000);
            Debug.Log("Did not Hit");
        }

    }
    //recursively call this function each time a pierce occurs
    private void EnvyPierce(RaycastHit obj, Vector3 fireAngle,Transform shotOrigin)
    {
     //   RaycastHit hit;
        RaycastHit backHit;
        RaycastHit nextObj;


        Debug.Log(obj.point);
        Ray backRay = new Ray(obj.point + fireAngle * 6, -1 * fireAngle * 6);
      
            obj.collider.Raycast(backRay, out backHit, 1000);
       // Debug.Log("stinky");
        Debug.Log(backHit.collider);
        Debug.Log(backHit.point);
      
            if (backHit.collider == null ||backHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {

                return;
            }
        
        

        if (backHit.collider!=null && backHit.transform.parent)
        {
           // Debug.Log(backHit.transform.gameObject.name);
            EntityController ShotEntity;
            ShotEntity = backHit.transform.parent.GetComponent<EntityController>();
            if (ShotEntity)
            {
                ShotEntity.Damage(20, shotOrigin);

            }
          

        }
        //then get next  object, 
      
       // Debug.DrawRay(backHit.point, fireAngle, Color.green, 10000);
        if (Physics.Raycast(backHit.point, fireAngle, out nextObj, 1000)&& backHit.point != obj.point) {
          //  Debug.Log("stinky");
            if (nextObj.collider != null )
            {
                Debug.DrawRay(backHit.point, fireAngle, Color.green, 10000);

                EnvyPierce(nextObj, fireAngle, shotOrigin);
            }
            }
            else
            {
                return;
            }

            //pass obj into envy again

        
        
    } 
}
