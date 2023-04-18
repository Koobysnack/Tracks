using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class  AbsSinClass : ScriptableObject
{
    [Header("Sin Base attributes")]
    // Start is called before the first frame update
    [SerializeField]
    protected bool sinFull;
    protected float chargeMax;
    protected float chargeCurrent;


    public abstract void SinFire(GameObject shotOrigin);

    public virtual void IncCharge( float value)
    {
       
        
        if (chargeCurrent +value > chargeMax)
        {
            chargeCurrent = chargeMax;
            SetSinFull(true);
        }
        else
        {
            chargeCurrent += value;
        }
    }


    public virtual void SetSinFull(bool value)
    {
        sinFull = value;
    }

    public virtual void DeIncCharge(float value)
    {
        SetSinFull(false);
        if (chargeCurrent - value > 0)
        {
            chargeCurrent -= value;
        }
        else
        {
            chargeCurrent = 0;
        }
    }


    public virtual bool GetSinFull()
    {
        return sinFull;
    }

}
