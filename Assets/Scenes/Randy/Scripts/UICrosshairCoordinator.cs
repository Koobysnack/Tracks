using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class UICrosshairCoordinator : MonoBehaviour
{
    private UICrosshairManager UICMan;
    private List<Image> bulletDisplay = new List<Image>();
    [SerializeField] private GameObject uiPivot;
    [SerializeField] private Image imagePrefab;
    Quaternion endAngle;
    bool rotating;
    List<Quaternion> angles = new List<Quaternion>();

    private void Awake()
    {
        rotating = false;    
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
        
    }

    void InstantiateBulletDisplay()
    {
        for (int i = 0; i < 7; i++)
        {
            Image newBullet = Instantiate(imagePrefab, uiPivot.transform, false);
            newBullet.rectTransform.anchoredPosition = new Vector2(UICMan.radius * Mathf.Sin(360f / 7f * i * Mathf.Deg2Rad), UICMan.radius * Mathf.Cos(360f / 7f * i * Mathf.Deg2Rad));
            bulletDisplay.Add(newBullet);
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
        while (time < UICMan.duration)
        {
            uiPivot.transform.rotation = Quaternion.Lerp(currentAngle, angles[i], (time - i * UICMan.duration / angles.Count) / (UICMan.duration * (i + 1) / angles.Count) );
            time += Time.deltaTime;
            if((time - i * UICMan.duration / angles.Count) >= UICMan.duration * (i + 1) / angles.Count)
                currentAngle = angles[i++];
            yield return null;
        }
        // Finalize rotation so it doesn't end at a weird spot
        uiPivot.transform.rotation = angles[angles.Count - 1];
        angles.Clear();
        rotating = false;
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
        while (time < UICMan.duration)
        {
            uiPivot.transform.rotation = Quaternion.Lerp(startAngle, endAngle, time / UICMan.duration);
            time += Time.deltaTime;
            yield return null;
        }
        // Finalize rotation so it doesn't end at a weird spot
        uiPivot.transform.rotation = Quaternion.Euler(0, 0, target);
        rotating = false;
    }

    //public IEnumerator RotateTest(float target)
    //{
    //    float speed = 2f;
    //    Vector3 targetDirection = new Vector3(Mathf.Cos(target), Mathf.Sin(target));
    //    while (uiPivot.transform.rotation != targetDirection)
    //}
}
