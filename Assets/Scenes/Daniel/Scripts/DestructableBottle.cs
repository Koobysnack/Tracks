using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBottle : DestructableController
{
    // Start is called before the first frame update
    public GameObject vfxshatter;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override IEnumerator DeathRattle()
    {
          print("glass shatter");
        Instantiate(vfxshatter, transform);
    //    Destroy(gameObject);

        yield return null;
           Destroy(gameObject);
    }
    protected override void Die()
    {
        GlassShatterSFX();
      StartCoroutine( DeathRattle());
     //   Instantiate(vfxshatter, transform);
    //  Destroy(gameObject);
     //   DeathRattle();
    }
    public override void Damage(float damage, Transform opponent = null)
    {
        print("Basic Bottle Damaged");
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void GlassShatterSFX()
    {
        string eventName = "event:/SFX/Items/Small Glass Break";

        var sound = FMODUnity.RuntimeManager.CreateInstance(eventName);
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        MusicManager.instance.TriggerSFX(sound, 8);
        //sound.start();
        //sound.release();
    }
}
