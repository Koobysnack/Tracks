using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;



public abstract class  AbsSinClass : MonoBehaviour 
{

    // Start is called before the first frame update
    [SerializeField] float cooldown;
    [SerializeField] float damage;
    [SerializeField] string sfx;

    public bool showDebugInfo;
    [ShowIf("showDebugInfo")] public bool ready;
    [SerializeField, ShowIf("showDebugInfo")] float runningCooldown;

    protected virtual void Awake()
    {
        ready = true;
        runningCooldown = 0;
    }

    public abstract void SinFire(Transform shotOrigin);

    protected IEnumerator StartCooldown()
    {
        runningCooldown = cooldown;
        ready = false;
        while (runningCooldown > 0)
        {
            runningCooldown -= Time.deltaTime;
            yield return null;
        }
        ready = true;
    }
}
