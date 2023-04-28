using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UIAmmoManager : MonoBehaviour
{
    public static UIAmmoManager instance;

    public UIAmmoController controller;
    public UIAmmoCoordinator coordinator;
    public float radius;
    public float rotateDuration;
    public float hideTimer;
    public float showHideSpeed;

    [Foldout("UI Bullet Sprites")] public Sprite loadedBullet;
    [Foldout("UI Bullet Sprites")] public Sprite firedBullet;

    [Foldout("BulletColors")] public Color normalBulletColor;
    [Foldout("BulletColors")] public Color prideBulletColor;
    [Foldout("BulletColors")] public Color greedBulletColor;
    [Foldout("BulletColors")] public Color wrathBulletColor;
    [Foldout("BulletColors")] public Color envyBulletColor;
    [Foldout("BulletColors")] public Color lustBulletColor;
    [Foldout("BulletColors")] public Color gluttonyBulletColor;
    [Foldout("BulletColors")] public Color slothBulletColor;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAmmoPanel()
    {
        controller.ShowAmmoPanel();
    }

    public void HideAmmoPanel()
    {
        coordinator.ForceHideDisplay();
    }

    public void RotateTo(int chamber, float direction)
    {
        controller.RotateTo(chamber, direction);
    }

    public void FireBullet(int chamber)
    {
        coordinator.FireBullet(chamber);
    }

    public void Reload()
    {
        coordinator.Reload();
    }
}
