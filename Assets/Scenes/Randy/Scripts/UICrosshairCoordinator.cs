using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class UICrosshairCoordinator : MonoBehaviour
{
    [SerializeField] private UICrosshairManager UICMan;
    private List<Image> bulletDisplay = new List<Image>();
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
        UICMan = UICrosshairManager.instance;
        print(UICMan);
        InstantiateBulletDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiPivotRT.position.y != (showAllBullets ? UICMan.radius + 10 : 0))
        {
            Vector2 currentPos = uiPivotRT.position;
            Vector2 targetPos = new Vector2(uiPivotRT.position.x, showAllBullets ? UICMan.radius + 10 : 0);
            float step = 400f * Time.deltaTime;
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
            newBullet.rectTransform.anchoredPosition = new Vector2(UICMan.radius * Mathf.Sin(360f / 7f * i * Mathf.Deg2Rad), UICMan.radius * Mathf.Cos(360f / 7f * i * Mathf.Deg2Rad));
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
            newBullet.rectTransform.anchoredPosition = new Vector2(UICMan.radius * Mathf.Sin(360f / 7f * i * Mathf.Deg2Rad), UICMan.radius * Mathf.Cos(360f / 7f * i * Mathf.Deg2Rad));
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
        while (time < UICMan.rotateDuration)
        {
            uiPivot.transform.rotation = Quaternion.Lerp(currentAngle, angles[i], (time - i * UICMan.rotateDuration / angles.Count) / (UICMan.rotateDuration * (i + 1) / angles.Count) );
            time += Time.deltaTime;
            if((time - i * UICMan.rotateDuration / angles.Count) >= UICMan.rotateDuration * (i + 1) / angles.Count)
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
            timeUntilHidden = UICMan.hideTimer;
            yield break;
        }
        hideTimerStarted = true;
        timeUntilHidden = UICMan.hideTimer;
        while (timeUntilHidden >= 0f)
        {
            timeUntilHidden -= Time.deltaTime;
            yield return null;
        }
        hideTimerStarted = false;
        showAllBullets = false;
    }

    public void FireBullet(int chamber)
    {
        bulletDisplay[chamber].sprite = UICMan.firedBullet;
    }

    public void Reload()
    {
        foreach(Image bullet in bulletDisplay)
        {
            bullet.sprite = UICMan.loadedBullet;
        }
    }

    // Old version
    public IEnumerator RotateChambers(float target)
    {
        Quaternion startAngle = uiPivot.transform.rotation;
        endAngle = Quaternion.Euler(0, 0, target);
        if (rotating)
        {
            yield break;
        }
        rotating = true;
        float time = 0;
        while (time < UICMan.rotateDuration)
        {
            uiPivot.transform.rotation = Quaternion.Lerp(startAngle, endAngle, time / UICMan.rotateDuration);
            time += Time.deltaTime;
            yield return null;
        }
        // Finalize rotation so it doesn't end at a weird spot
        uiPivot.transform.rotation = Quaternion.Euler(0, 0, target);
        rotating = false;
    }
}
