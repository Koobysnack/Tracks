using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitScanRaycast : MonoBehaviour
{
    public EnvySin TestEnvy;
    public int ChamberNum = 0;

    private PlayerInputActions pInput ;


    public void SetChamber(int change)
    {
        ChamberNum = change;
    }

    void Awake()
    {
        pInput = new PlayerInputActions();
        pInput.Gun.Fire.performed += PierceRayCaster;
        pInput.Gun.AltFire.performed += SinWrapper;
     
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TestEnvy.SinFire(transform.gameObject);
    }
    //Non piercing raycast

    private void OnEnable()
    {
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
    }

    public void SinWrapper(InputAction.CallbackContext context)
    {
        
        print("deez");
        if (ChamberNum = 4)
        {
            TestEnvy.SinFire(transform.gameObject);
        }
    }

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
    public void PierceRayCaster(InputAction.CallbackContext context)
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
