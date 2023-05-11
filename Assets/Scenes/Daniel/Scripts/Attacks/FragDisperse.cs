using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragDisperse : HitScanRaycast
{
    //Use this to create a random spread of x pellets in a specific area
    // Start is called before the first frame update

    [SerializeField]
    protected int pelletnum;
    protected bool blast;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FragCone(Transform coneDir, float coneBase,float coneHeight)
    {

    }

    public void FragCircle(Transform circleOrigin,float circleRadius)
    {

    }
    



}
