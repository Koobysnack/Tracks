using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BulletType { NORMAL, PRIDE, GREED, WRATH, ENVY, LUST, GLUTTONY, SLOTH }
public class Revolver : MonoBehaviour
{
    [System.Serializable]
    public class Bullet
    {
        [HideInInspector] public string name;
        public BulletType type;
        public bool loaded;
    }
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
        ammoMan.HideAmmoPanel();
        hitRC.PierceRayCaster();
        StartCoroutine(NormalShootCooldown());
        GunfireSFX();
        cylinder[currentBullet].loaded = false;
        ammoMan.FireBullet(currentBullet);
        CycleBullet();
    }

    void CycleBullet()
    {
        if (!cylinder.Exists(bullet => bullet.loaded == true))
        {
            currentBullet = (currentBullet + 1) % cylinder.Count;
            ammoMan.RotateTo(currentBullet, 1);
            return;
        }
            
        while (!cylinder[currentBullet].loaded)
        {
            currentBullet = (currentBullet + 1) % cylinder.Count;
        }
        ammoMan.RotateTo(currentBullet, 1);
    }

    void SelectBullet(InputAction.CallbackContext context) // Less than ideal argument but this is a quick prototype
    {
        float direction = context.ReadValue<float>();
        if (!cylinder.Exists(bullet => bullet.loaded == true))
        {
            currentBullet = (currentBullet + (int)Mathf.Sign(direction) + cylinder.Count) % cylinder.Count;
            ammoMan.ShowAmmoPanel();
            ammoMan.RotateTo(currentBullet, Mathf.Sign(direction));
            return;
        }

        Mathf.Sign(direction);

        do
        {
            currentBullet = (currentBullet + (int)Mathf.Sign(direction) + cylinder.Count) % cylinder.Count;
        }
        while (!cylinder[currentBullet].loaded);
        ammoMan.ShowAmmoPanel();
        ammoMan.RotateTo(currentBullet, Mathf.Sign(direction));
    }

    void Reload(InputAction.CallbackContext context = default(InputAction.CallbackContext))
    {
        foreach (Bullet bullet in cylinder)
        {
            bullet.loaded = true;
        }
        currentBullet = 0;
        ammoMan.Reload();
        ammoMan.ShowAmmoPanel();
        ammoMan.RotateTo(currentBullet, 1);
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
        //if () eventName = "event:/SFX/Player/GunshotEmpty";
        eventName = "event:/SFX/Player/Gunshot";

        var sound = FMODUnity.RuntimeManager.CreateInstance(eventName);
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        sound.start();
        sound.release();
    }

}
