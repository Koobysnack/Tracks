using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvySin : AbsSinClass
{
    public bool AltAbility;
    [SerializeField]
    //private EntityController EntityRef;
    private LayerMask lMask;
    private LayerMask xRayed;
    [SerializeField]
    private int pierceMax;
    [SerializeField]
    private int pierceCurr;
    public Vector3 XrayExtents;



    void OnEnable()
    {
        //add "revealed" 
        lMask = LayerMask.GetMask("Enemy,Ground");
    }


    public override void SinFire(Transform shotOrigin)
    {
        StartCoroutine(StartCooldown());
        RaycastHit hit;
        lMask = LayerMask.GetMask("Enemy,Ground");
        SinFireSFX();

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(shotOrigin.position, shotOrigin.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(shotOrigin.position, shotOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red, 1000);
            Debug.Log("Did Hit");

            //  RaycastHit pierceHit;

            // print(hit);
           // if (AltAbility)
           // {
            //    Physics.BoxCastAll()
         //   }
            pierceCurr += 1;
            EnvyPierce(hit, shotOrigin.TransformDirection(Vector3.forward), shotOrigin);

        }
        else
        {
            Debug.DrawRay(shotOrigin.position, shotOrigin.TransformDirection(Vector3.forward) * 1000, Color.white, 1000);
            Debug.Log("Did not Hit");
        }

    }
    //recursively call this function each time a pierce occurs

    private void  EnvyPierce(RaycastHit obj, Vector3 fireAngle, Transform shotOrigin)
    {

        //   RaycastHit hit;
        RaycastHit backHit;
        RaycastHit nextObj;
        Ray backRay = new Ray(obj.point + fireAngle * 6, -1 * fireAngle * 6);

        obj.collider.Raycast(backRay, out backHit, 1000);
        // Debug.Log("stinky");
  

        if (backHit.collider == null || backHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return;
        }

        if (backHit.collider != null && backHit.transform.parent)
        {
            // Debug.Log(backHit.transform.gameObject.name);
            
            EntityController ShotEntity;
            ShotEntity = backHit.transform.parent.GetComponent<EntityController>();
            if (ShotEntity)
            {
                ShotEntity.Damage(damage, shotOrigin);

            }
            //Boxcast of x size with only enemy layer
            // change enemy layer to revealed layer
        }

        //then get next  object, 

        // Debug.DrawRay(backHit.point, fireAngle, Color.green, 10000);
        if (Physics.Raycast(backHit.point, fireAngle, out nextObj, 1000) && backHit.point != obj.point)
        {
            //  Debug.Log("stinky");
            if (nextObj.collider != null)
            {
                Debug.DrawRay(backHit.point, fireAngle, Color.green, 10000);
                pierceCurr += 1;
                EnvyPierce(nextObj, fireAngle, shotOrigin);
            }
        }
        else
        {
            return;
        }

        //pass obj into envy again



    }

   // IEnumerator EnemyReveal()
  //  {

   // }
}
