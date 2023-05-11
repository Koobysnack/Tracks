using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NoSin : AbsSinClass
{

    [SerializeField]
    private EntityController EntityRef;

    public override void SinFire(Transform shotOrigin)
    {
        return;
    }

}