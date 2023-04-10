using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UICrosshairManager : MonoBehaviour
{
    public static UICrosshairManager instance;

    [Foldout("BulletColors")] public Color normalBulletColor;
    [Foldout("BulletColors")] public Color prideBulletColor;
    [Foldout("BulletColors")] public Color greedBulletColor;
    [Foldout("BulletColors")] public Color wrathBulletColor;
    [Foldout("BulletColors")] public Color envyBulletColor;
    [Foldout("BulletColors")] public Color lustBulletColor;
    [Foldout("BulletColors")] public Color gluttonyBulletColor;
    [Foldout("BulletColors")] public Color slothBulletColor;

    public UICrosshairController controller;
    public float radius;
    public float duration;

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

    public void RotateTo(int chamber, float direction)
    {
        controller.RotateTo(chamber, direction);
    }
}
