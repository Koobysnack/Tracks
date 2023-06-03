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
        xRayed = LayerMask.GetMask("Revealed");
    }


    public override void SinFire(Transform shotOrigin)
    {
        pierceCurr = 0;
        print("SHOOTING");
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
  

        if (backHit.collider == null || backHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || pierceCurr >7)
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
            RaycastHit[] RevealedEnemies = Physics.BoxCastAll(backHit.point + backHit.point.normalized * XrayExtents.x, XrayExtents, fireAngle, Quaternion.LookRotation(fireAngle, Vector3.up), 2f, LayerMask.GetMask("Enemy"));
             Debug.DrawRay(backHit.point+backHit.point.normalized* XrayExtents.x, fireAngle, Color.green, 10000);
            StartCoroutine("EnemyReveal",RevealedEnemies);
            
            //Boxcast of x size with only enemy layer
            // Ienumerate reveal duration
        }

        //then get next  object, 

        // Debug.DrawRay(backHit.point, fireAngle, Color.green, 10000);
        if (Physics.Raycast(backHit.point, fireAngle, out nextObj, 1000) && backHit.point != obj.point)
        {
            //  Debug.Log("stinky");
            if (nextObj.collider != null)
            {
              //  Debug.DrawRay(backHit.point, fireAngle, Color.green, 10000);
                pierceCurr += 1;
                EnvyPierce(nextObj, fireAngle, shotOrigin);
            }
        }
        else
        {
            return;
        }

 



    }

   IEnumerator EnemyReveal(RaycastHit[] revealList )
   {
        foreach(RaycastHit blip in revealList)
        {
            blip.collider.gameObject.layer = LayerMask.NameToLayer("Revealed");
            print("hello");
            yield return null;
        }
        yield return new WaitForSeconds(7);
        foreach (RaycastHit blip in revealList)
        {
            blip.collider.gameObject.layer = LayerMask.NameToLayer("Enemy");
            print("bybye");
            yield return null;
        }
    }
}
