using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameObject envyVFX;
    public ParticleSystem envyRipple;
    public ParticleSystem envyFlash;
    public GameObject Gunbarrel;



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

             print(hit.transform);
          
            pierceCurr += 1;
            StartCoroutine(MakeLazerEffect(shotOrigin, hit, true, shotOrigin.TransformDirection(Vector3.forward), true));
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


            //Instantiate ripple
        }

        //then get next  object, 

        // Debug.DrawRay(backHit.point, fireAngle, Color.green, 10000);
        if (Physics.Raycast(backHit.point, fireAngle, out nextObj, 1000) && backHit.point != obj.point)
        {
            //  Debug.Log("stinky");
            if (nextObj.collider != null)
            {
                //instatiate beam
                //Instantiate ripple


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
             // blip.collider.gameObject.layer = LayerMask.NameToLayer("Revealed");

            var outline = blip.collider.gameObject.GetComponent<Outline>();
            outline = outline == null ? blip.collider.gameObject.transform.parent.GetComponent<Outline>() : outline;
            if (outline)
            {
                outline.enabled = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(7);
        foreach (RaycastHit blip in revealList)
        {
            //   blip.collider.gameObject.layer = LayerMask.NameToLayer("Enemy");
            var outline = blip.collider.gameObject.GetComponent<Outline>();
            outline = outline == null ? blip.collider.gameObject.transform.parent.GetComponent<Outline>() : outline;
            if (outline)
            {
                outline.enabled = false ;
            }
            
            yield return null;
        }
    }
    IEnumerator MakeLazerEffect(Transform firstImpact, RaycastHit secondImpact,bool Muzzleflash,Vector3 fireAngle,bool secondHit )
    {
        print("the sussiest");
        if (Muzzleflash)
        {
            //instatntiate flash
            ParticleSystem flashinstance = Instantiate(envyFlash, firstImpact.position,  Quaternion.LookRotation(fireAngle, Vector3.up));

        }
        yield return null;
        ParticleSystem FirstRipple = Instantiate(envyRipple, firstImpact.position, Quaternion.LookRotation(fireAngle, Vector3.up));
        //instantiate ripple
        yield return null;
        GameObject Beam = Instantiate(envyVFX, firstImpact.position, Quaternion.LookRotation(fireAngle, Vector3.up));
        yield return null;
        LineRenderer tempLine = Beam.GetComponentInChildren<LineRenderer>();
        yield return null;
        tempLine.SetPositions(new Vector3[] {  firstImpact.position,secondImpact.point });
        yield return null;
       // tempLine.enabled=true;
        yield return null;
        if (secondHit)
        {
            ParticleSystem SecondRipple = Instantiate(envyRipple, secondImpact.point, Quaternion.LookRotation(fireAngle, Vector3.up));
        }
        yield return new WaitForSeconds(1);
      //  Destroy(Beam);
        
    }

}
