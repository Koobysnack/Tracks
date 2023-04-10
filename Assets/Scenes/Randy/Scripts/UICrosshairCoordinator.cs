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

    private void Awake()
    {
        
        
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
    }

    public IEnumerator RotateChamber(float target)
    {
        float time = 0;
        Quaternion startAngle = uiPivot.transform.rotation;
        Quaternion endAngle = Quaternion.Euler(0, 0, target);
        while (time < UICMan.duration)
        {
            uiPivot.transform.rotation = Quaternion.Lerp(startAngle, endAngle, time/UICMan.duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
