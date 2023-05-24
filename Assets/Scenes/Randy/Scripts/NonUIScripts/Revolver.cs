using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : MonoBehaviour
{
    [Min(0.01f), SerializeField] float normalFireRate;
    [SerializeField] PlayerController pController;
    public int currentBullet;
    public List<Bullet> cylinder = new List<Bullet>();
    HitScanRaycast hitRC;
    UIAmmoManager ammoMan;
    SinManager sinMan;
    // PlayerInputActions pInput;
    bool ready;
    bool useSin;
    

    void OnValidate()
    {
        foreach (Bullet b in cylinder) b.name = "Bullet " + cylinder.IndexOf(b) + ": " + b.type.ToString();
    }

    void Awake()
    {
        hitRC = GetComponent<HitScanRaycast>();
        // pInput = new PlayerInputActions();
        // pInput.Gun.Fire.performed += Shoot;
        // pInput.Gun.AltFire.performed += AltShoot;
        // pInput.Gun.RotateCylinder.performed += SelectBullet;
        // pInput.Gun.Reload.performed += Reload;
    }

    // void OnEnable()
    // {
    //     pInput.Enable();
    // }

    // void OnDisable()
    // {
    //     pInput.Disable();
    // }

    void Start()
    {
        foreach (Bullet bullet in cylinder)
        {
            bullet.loaded = true;
        }
        ready = true;
        useSin = false;
    }

    void Update()
    {
        if(UIAmmoManager.instance != null && ammoMan == null)
            ammoMan = UIAmmoManager.instance;
        if(SinManager.instance != null && sinMan == null)
        {
            sinMan = SinManager.instance;
            LateInit();
        }
    }

    void LateInit()
    {
        foreach (Bullet bullet in cylinder)
        {
            bullet.type = sinMan.GetSinObj(Sin.NORMAL);
        }
        if(ammoMan != null)
            ammoMan.RequestBulletUpdate();
    }

    public void Shoot()//InputAction.CallbackContext context)
    {
        if (!ready)
            return;

        if (pController.currentAmmoCount < 1)
        {
            // Warn out of ammo
            return;
        }

        if (cylinder[currentBullet].loaded == false)
        {
            Reload();
            return;
        }

        if (!(sinMan.GetSinEnum(cylinder[currentBullet].type) == Sin.NORMAL || !useSin)) // only when using a bullet with sin infusion and RMB held
            DoShootSin();
        else
            DoShootBullet();
    }

    public void ReadySin()//InputAction.CallbackContext context)
    {
        useSin = true;
    }

    public void CancelSin()
    {
        useSin = false;
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

    public void SelectBullet(float direction)//InputAction.CallbackContext context)
    {
        //float direction = context.ReadValue<float>();
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
            if (ammoMan != null)
            {
                ammoMan.ShowAmmoPanel();
                ammoMan.RotateTo(currentBullet, Mathf.Sign(direction));
            }
        }
    }

    public void Reload()//InputAction.CallbackContext context = default(InputAction.CallbackContext))
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

    public void SwapBullet(int chamber, AbsSinClass newType)
    {
        cylinder[chamber].type = newType;
    }

    void DoShootBullet()
    {
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

    void DoShootSin()
    {
        Bullet bullet = cylinder[currentBullet];
        if (!bullet.type.ready)
        {
            // Warn Sin not ready
            return;
        }

        if (pController.currentAmmoCount < 1)
        {
            // Warn out of ammo
            return;
        }

        if (cylinder[currentBullet].loaded == false)
        {
            Reload();
            return;
        }

        bullet.type.SinFire(transform);
        bullet.loaded = false;
        if (ammoMan != null)
        {
            ammoMan.HideAmmoPanel();
            ammoMan.FireBullet(currentBullet);
        }
        CycleBullet();
    }
}
