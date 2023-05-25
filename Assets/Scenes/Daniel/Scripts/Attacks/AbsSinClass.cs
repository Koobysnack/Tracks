using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;



public abstract class  AbsSinClass : MonoBehaviour 
{

    // Start is called before the first frame update
    [SerializeField] protected float cooldown;
    [SerializeField] protected float damage;
    [SerializeField] protected string sfx;

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

    protected void SinFireSFX()
    {
        if (string.IsNullOrEmpty(sfx))
            return;
        var sound = FMODUnity.RuntimeManager.CreateInstance(sfx);
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        if (MusicManager.instance != null)
            MusicManager.instance.TriggerSFX(sound, 8);
        //sound.start();
        //sound.release();
    }
}
