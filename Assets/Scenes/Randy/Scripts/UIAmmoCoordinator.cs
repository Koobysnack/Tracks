using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class UIAmmoCoordinator : MonoBehaviour
{
    [SerializeField] private UIAmmoManager UIAmmoMan;
    List<Image> bulletDisplay = new List<Image>();
    [SerializeField] private GameObject uiPivot;
    [SerializeField] private Image imagePrefab;

    // Rotation vars
    Quaternion endAngle;
    bool rotating;
    List<Quaternion> angles = new List<Quaternion>();

    // Hiding vars
    bool showAllBullets;
    bool hideTimerStarted;
    float timeUntilHidden;
    RectTransform uiPivotRT;
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        rotating = false;
        hideTimerStarted = false;
        showAllBullets = false;
        uiPivotRT = uiPivot.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UIAmmoMan = UIAmmoManager.instance;
        InstantiateBulletDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiPivotRT.position.y != (showAllBullets ? UIAmmoMan.radius + 10 : 0))
        {
            Vector2 currentPos = uiPivotRT.position;
            Vector2 targetPos = new Vector2(uiPivotRT.position.x, showAllBullets ? UIAmmoMan.showPivotHeight * canvas.scaleFactor : 0);
            float step = UIAmmoMan.showHideSpeed * Time.deltaTime;
            uiPivotRT.position = Vector2.MoveTowards(currentPos, targetPos, step);
        }
    }

    void InstantiateBulletDisplay()
    {
        foreach (Image obj in uiPivot.GetComponentsInChildren<Image>())
        {
            DestroyImmediate(obj.gameObject);
        }
        for (int i = 0; i < 7; i++)
        {
            Image newBullet = Instantiate(imagePrefab, uiPivot.transform, false);
            newBullet.rectTransform.anchoredPosition = new Vector2(UIAmmoMan.radius * Mathf.Sin(360f / 7f * i * Mathf.Deg2Rad), UIAmmoMan.radius * Mathf.Cos(360f / 7f * i * Mathf.Deg2Rad));
            bulletDisplay.Add(newBullet);
        }
        uiPivot.GetComponentInChildren<Image>().color = Color.blue;
    }

    [Button]
    void VisualizeBullets()
    {
        if (uiPivot.GetComponentsInChildren<Image>().Length != 0)
        {
            foreach (Image obj in uiPivot.GetComponentsInChildren<Image>())
            {
                DestroyImmediate(obj.gameObject);
            }
            return;
        }
        for (int i = 0; i < 7; i++)
        {
            Image newBullet = Instantiate(imagePrefab, uiPivot.transform, false);
            newBullet.rectTransform.anchoredPosition = new Vector2(UIAmmoMan.radius * Mathf.Sin(360f / 7f * i * Mathf.Deg2Rad), UIAmmoMan.radius * Mathf.Cos(360f / 7f * i * Mathf.Deg2Rad));
        }
        uiPivot.GetComponentInChildren<Image>().color = Color.blue;
    }

    public IEnumerator RotateChamber(float target, float direction)
    {
        float pivotAngle = angles.Count > 0 ? angles[angles.Count - 1].eulerAngles.z : uiPivot.transform.rotation.eulerAngles.z;
        pivotAngle = (pivotAngle + 360) % 360;
        float deltaAngle = Mathf.DeltaAngle(pivotAngle, target);
        if (Mathf.Sign(deltaAngle) != Mathf.Sign(direction))
            angles.Add(Quaternion.Euler(0,0, (pivotAngle + deltaAngle / 2 + 180) % 360));
        angles.Add(Quaternion.Euler(0, 0, target));

        if (rotating)
            yield break;
        rotating = true;
        
        Quaternion currentAngle = uiPivot.transform.rotation;
        float time = 0;
        int i = 0;
        while (time < UIAmmoMan.rotateDuration)
        {
            uiPivot.transform.rotation = Quaternion.Lerp(currentAngle, angles[i], (time - i * UIAmmoMan.rotateDuration / angles.Count) / (UIAmmoMan.rotateDuration * (i + 1) / angles.Count) );
            time += Time.deltaTime;
            if((time - i * UIAmmoMan.rotateDuration / angles.Count) >= UIAmmoMan.rotateDuration * (i + 1) / angles.Count)
                currentAngle = angles[i++];
            yield return null;
        }
        // Finalize rotation so it doesn't end at a weird spot
        uiPivot.transform.rotation = angles[angles.Count - 1];
        angles.Clear();
        rotating = false;
    }

    public void ShowDisplay()
    {
        showAllBullets = true;
        StartCoroutine(HideDisplay());
    }

    public IEnumerator HideDisplay()
    {
        if (hideTimerStarted)
        {
            timeUntilHidden = UIAmmoMan.hideTimer;
            yield break;
        }
        hideTimerStarted = true;
        timeUntilHidden = UIAmmoMan.hideTimer;
        while (timeUntilHidden >= 0f)
        {
            timeUntilHidden -= Time.deltaTime;
            yield return null;
        }
        hideTimerStarted = false;
        showAllBullets = false;
    }

    public void ForceHideDisplay()
    {
        showAllBullets = false;
    }

    public void FireBullet(int chamber)
    {
        bulletDisplay[chamber].sprite = UIAmmoMan.firedBullet;
    }

    public void Reload()
    {
        foreach(Image bullet in bulletDisplay)
        {
            bullet.sprite = UIAmmoMan.loadedBullet;
        }
    }

    public void UpdateBulletDisplay(List<Bullet> newLoadout)
    {
        // To refactor: either move to different controller or somewhere else
        for(int i = 0; i < newLoadout.Count; i++)
        {
            Sin s = SinManager.instance.GetSinEnum(newLoadout[i].type);
            Color newColor;
            switch(s)
            {
                case Sin.NORMAL:
                    newColor = UIAmmoMan.normalBulletColor;
                    break;
                case Sin.PRIDE:
                    newColor = UIAmmoMan.prideBulletColor;
                    break;
                case Sin.GREED:
                    newColor = UIAmmoMan.greedBulletColor;
                    break;
                case Sin.WRATH:
                    newColor = UIAmmoMan.wrathBulletColor;
                    break;
                case Sin.ENVY:
                    newColor = UIAmmoMan.envyBulletColor;
                    break;
                case Sin.LUST:
                    newColor = UIAmmoMan.lustBulletColor;
                    break;
                case Sin.GLUTTONY:
                    newColor = UIAmmoMan.gluttonyBulletColor;
                    break;
                case Sin.SLOTH:
                    newColor = UIAmmoMan.slothBulletColor;
                    break;
                default:
                    continue;
            }
            bulletDisplay[i].color = newColor;
            // When UI elements come in, update icon too
        }
    }
}
