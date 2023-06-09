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
    public float showPivotHeight;
    public float showHideSpeed;

    [Foldout("UI Bullet Sprites")] public Sprite loadedBullet;
    [Foldout("UI Bullet Sprites")] public Sprite firedBullet;
    [Foldout("UI Bullet Sprites")] public Sprite prideIcon;
    [Foldout("UI Bullet Sprites")] public Sprite greedIcon;
    [Foldout("UI Bullet Sprites")] public Sprite wrathIcon;
    [Foldout("UI Bullet Sprites")] public Sprite envyIcon;
    [Foldout("UI Bullet Sprites")] public Sprite lustIcon;
    [Foldout("UI Bullet Sprites")] public Sprite gluttonyIcon;
    [Foldout("UI Bullet Sprites")] public Sprite slothIcon;

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

    public void RequestBulletUpdate()
    {
        coordinator.UpdateBulletDisplay();
        return;
    }
}
