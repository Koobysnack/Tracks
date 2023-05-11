using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : MonoBehaviour
{
    [Min(0.01f), SerializeField] float normalFireRate;
    public int currentBullet;
    public List<Bullet> cylinder = new List<Bullet>();
    HitScanRaycast hitRC;
    UIAmmoManager ammoMan;
    PlayerInputActions pInput;
    bool ready;

    void OnValidate()
    {
        foreach (Bullet b in cylinder) b.name = "Bullet " + cylinder.IndexOf(b) + ": " + b.type.ToString();
    }

    void Awake()
    {
        hitRC = GetComponent<HitScanRaycast>();
        pInput = new PlayerInputActions();
        pInput.Gun.Fire.performed += Shoot;
        pInput.Gun.AltFire.performed += AltShoot;
        pInput.Gun.RotateCylinder.performed += SelectBullet;
        pInput.Gun.Reload.performed += Reload;
    }

    void OnEnable()
    {
        pInput.Enable();
    }

    void OnDisable()
    {
        pInput.Disable();
    }

    void Start()
    {
        foreach (Bullet bullet in cylinder)
        {
            bullet.loaded = true;
        }
        ready = true;
    }

    void Update()
    {
        if (UIAmmoManager.instance != null && ammoMan == null)
            ammoMan = UIAmmoManager.instance;
    }

    void Shoot(InputAction.CallbackContext context)
    {
        if (!ready)
            return;

        if (cylinder[currentBullet].loaded == false)
        {
            Reload();
            return;
        }
        
        hitRC.PierceRayCaster();
        StartCoroutine(NormalShootCooldown());
        GunfireSFX();
        cylinder[currentBullet].loaded = false;
        if (ammoMan != null)
        {
            ammoMan.HideAmmoPanel();
            ammoMan.FireBullet(currentBullet);
        }
        CycleBullet();
    }

    void AltShoot(InputAction.CallbackContext context)
    {
        if (!ready)
            return;

        if (cylinder[currentBullet].loaded == false)
        {
            Reload();
            return;
        }
        GunfireSFX();
        cylinder[currentBullet].type.SinFire(transform);
        cylinder[currentBullet].loaded = false;
        if (ammoMan != null)
        {
            ammoMan.HideAmmoPanel();
            ammoMan.FireBullet(currentBullet);
        }
        CycleBullet();
    }


    void CycleBullet()
    {
        if (!cylinder.Exists(bullet => bullet.loaded == true))
        {
            currentBullet = (currentBullet + 1) % cylinder.Count;
            if (ammoMan != null)
                ammoMan.RotateTo(currentBullet, 1);
            return;
        }
            
        while (!cylinder[currentBullet].loaded)
        {
            currentBullet = (currentBullet + 1) % cylinder.Count;
        }
        if (ammoMan != null)
            ammoMan.RotateTo(currentBullet, 1);
    }

    void SelectBullet(InputAction.CallbackContext context) // Less than ideal argument but this is a quick prototype
    {
        float direction = context.ReadValue<float>();
        if (!cylinder.Exists(bullet => bullet.loaded == true))
        {
            currentBullet = (currentBullet + (int)Mathf.Sign(direction) + cylinder.Count) % cylinder.Count;
            if (ammoMan != null)
            {
                ammoMan.ShowAmmoPanel();
                ammoMan.RotateTo(currentBullet, Mathf.Sign(direction));
            }
            return;
        }

        Mathf.Sign(direction);

        do
        {
            currentBullet = (currentBullet + (int)Mathf.Sign(direction) + cylinder.Count) % cylinder.Count;
        }
        while (!cylinder[currentBullet].loaded);
        {
            ammoMan.ShowAmmoPanel();
            ammoMan.RotateTo(currentBullet, Mathf.Sign(direction));
        }
    }

    void Reload(InputAction.CallbackContext context = default(InputAction.CallbackContext))
    {
        foreach (Bullet bullet in cylinder)
        {
            bullet.loaded = true;
        }
        currentBullet = 0;
        if (ammoMan != null)
        {
            ammoMan.Reload();
            ammoMan.ShowAmmoPanel();
            ammoMan.RotateTo(currentBullet, 1);
        }
    }

    IEnumerator NormalShootCooldown()
    {
        ready = false;
        yield return new WaitForSeconds(1f / normalFireRate);
        ready = true;
    }

    private void GunfireSFX()
    {
        string eventName;
        //if (no bullets) eventName = "event:/SFX/Player/GunshotEmpty";
        eventName = "event:/SFX/Player/Gunshot";

        var sound = FMODUnity.RuntimeManager.CreateInstance(eventName);
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        MusicManager.instance.TriggerSFX(sound, 8);
        //sound.start();
        //sound.release();
    }

}
