using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnPierce 
{
    // Anything that gets shot with a piercing ray should have this function defined and called
    // Return 
  public   float PierceCalc(float rayHardness);

    public void HitEffects();

}
