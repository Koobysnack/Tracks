using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHardness : MonoBehaviour, IOnHitScan
{
    // Start is called before the first frame update

    [SerializeField]
    private float hardnessVal = 2;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public float PierceCalc(float rayHardness)
    {

        return rayHardness - hardnessVal;
    }

    public void HitEffects()
    {
      MeshRenderer meshMan = gameObject.GetComponent<MeshRenderer>();
        meshMan.material.color = Color.red;  
    }
}
