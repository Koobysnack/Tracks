using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHardness : MonoBehaviour, IOnHitScan
{
    // Start is called before the first frame update

    [SerializeField]
    private float hardnessVal = 2;

    public int ColorTest = 0;
    
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
        switch (ColorTest) {

            case 0:
                meshMan.material.color = Color.red;
                break;
            case 1:
                meshMan.material.color = Color.blue;
                break;
            case 2:
                meshMan.material.color = Color.cyan;
                break;
            default:
                break;

        }
    }
}
