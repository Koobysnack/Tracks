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

    public List<Bullet> cylinder = new List<Bullet>();
    public int currentBullet;
    HitScanRaycast hitRC;
    UIAmmoManager ammoMan;
    PlayerInputActions pInput;

    void OnValidate()
    {
        foreach (Bullet b in cylinder) b.name = "Bullet " + cylinder.IndexOf(b) + ": " + b.type.ToString();
    }

    void Awake()
    {
        hitRC = GetComponent<HitScanRaycast>();
        ammoMan = UIAmmoManager.instance;
        pInput = new PlayerInputActions();
        pInput.Gun.Fire.performed += Shoot;
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
    }

    void Shoot(InputAction.CallbackContext context)
    {
        if (cylinder[currentBullet].loaded == false)
        {
            reload();
            return;
        }
        hitRC.PierceRayCaster();
        cylinder[currentBullet].loaded = false;
        ammoMan.FireBullet(currentBullet);
        CycleBullet();
    }

    void CycleBullet()
    {
        // Do not run function if no bullet is available
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

    void SelectBullet(float direction) // Less than ideal argument but this is a quick prototype
    {
        // Do not run function if no bullet is available
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

    void reload()
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
}
