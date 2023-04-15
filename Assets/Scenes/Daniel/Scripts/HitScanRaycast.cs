using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanRaycast : MonoBehaviour
{
    public EnvySin TestEnvy;
    //hypothetical bullet system
    //1. fire raycastall  
    //2. activate all effects of getting shot on objects that are hit

    //Bullet effects will be the icky part
    //
    //3. pass raycasthit array from step 1 into separate 

    //depending on what "each script only does one thing" means

    //chamber/bullet effect coordinator controls sins (sins all have separate class)
    //bullet controller tells coordinator to have x sin selected (select sin, update sin charge)
    //bullet manager tells controller to change sin selected, to call raycast with regular or sin effect

    // input -> manager -> controller -> sin x, y ,z  \ 
    //             \                                   -> raycast
    //              regular bullet                    /
    //

 
    // sin abstract class 
    // separate regular bullet handling
    // 

    //a lot of conjecture not much action
    //get a raycast working
    //
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TestEnvy.SinFire(transform.gameObject);
    }
    //Non piercing raycast

    

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
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
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
            Physics.Raycast(backHit.point, transform.TransformDirection(Vector3.forward), out hit2,Mathf.Infinity);

            Debug.DrawRay(backHit.point, transform.TransformDirection(Vector3.up), Color.blue);
            ObjectHardness objectPierceTest;
          objectPierceTest =  hit2.transform.gameObject.GetComponent<ObjectHardness>();
            if (objectPierceTest)
            {
                objectPierceTest.HitEffects();
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

    }
    // raycasthit array CastHitScanRay( gameobject tag ) 
    //RayCastAll, with physics set to querybackfaces
    //set up interface for shootables, and call the "GetShot" function or something,
    //
    //return raycasthit array (to be used in effect calculation
    //
}
