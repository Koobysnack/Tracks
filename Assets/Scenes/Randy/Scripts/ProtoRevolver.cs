using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoRevolver : MonoBehaviour
{
    public enum BulletType { NORMAL, PRIDE, GREED, WRATH, ENVY, LUST, GLUTTONY, SLOTH }
    [System.Serializable]
    public class Bullet
    {
        [HideInInspector] public string name;
        public BulletType type;
        public bool loaded;
    }
    public List<Bullet> cylinder = new List<Bullet>();
    public int currentBullet;

    private void OnValidate()
    {
        foreach (Bullet b in cylinder) b.name = "Bullet " + cylinder.IndexOf(b) + ": " + b.type.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Bullet bullet in cylinder)
        {
            bullet.loaded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            print("Normal Shot");
            Shoot();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            print("Selecting");
            SelectBullet(scroll);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            reload();
        }
    }

    void Shoot()
    {
        if(cylinder[currentBullet].loaded == false)
        {
            reload();
            return;
        }
        cylinder[currentBullet].loaded = false;
        UICrosshairManager.instance.FireBullet(currentBullet);
        CycleBullet();
    }

    void CycleBullet()
    {
        // Do not run function if no bullet is available
        if( !cylinder.Exists(bullet => bullet.loaded == true))
            return;
            

        while (!cylinder[currentBullet].loaded)
        {
            currentBullet = (currentBullet + 1) % cylinder.Count;
            print(currentBullet);
        }
        UICrosshairManager.instance.RotateTo(currentBullet, 1);
    }

    void SelectBullet(float direction) // Less than ideal argument but this is a quick prototype
    {
        // Do not run function if no bullet is available
        if (!cylinder.Exists(bullet => bullet.loaded == true))
            return;

        Mathf.Sign(direction);

        do
        {
            currentBullet = (currentBullet + (int)Mathf.Sign(direction) + cylinder.Count) % cylinder.Count;
        }
        while (!cylinder[currentBullet].loaded);
        UICrosshairManager.instance.ShowAmmoPanel();
        UICrosshairManager.instance.RotateTo(currentBullet, Mathf.Sign(direction));
    }

    void reload()
    {
        foreach (Bullet bullet in cylinder)
        {
            bullet.loaded = true;
        }
        currentBullet = 0;
        UICrosshairManager.instance.Reload();
        UICrosshairManager.instance.ShowAmmoPanel();
        UICrosshairManager.instance.RotateTo(currentBullet, 1);
    }
}
