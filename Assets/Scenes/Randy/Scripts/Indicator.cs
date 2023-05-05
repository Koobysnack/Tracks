using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    UIDamageIndicatorManager uiDIMan;
    float maxTime;
    float timer;
    bool timerActive;
    RectTransform indicatorTransform;
    CanvasGroup indicatorCanvasGroup;
    Transform target;
    Transform player;
    Quaternion rotationToTarget;
    Vector3 targetLocation;
    Action<Indicator> endIndication;
    Action<Transform> removeFromDict;
    Image indicatorSprite;

    void Awake()
    {
        uiDIMan = UIDamageIndicatorManager.instance;
        indicatorSprite = GetComponent<Image>();
        indicatorTransform = GetComponent<RectTransform>();
        indicatorCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void LateInit(Action<Indicator> release, Action<Transform> dictRemove)
    {
        endIndication = release;
        removeFromDict = dictRemove;
    }

    public void RegisterHit(Transform p, Transform t, float d, Color c)
    {
        player = p;
        target = t;
        maxTime = d;
        timer = maxTime;
        indicatorSprite.color = c;
        StartCoroutine(PointToTarget());
        StartCoroutine(StartTimer());
    }

    public void ResetTimer()
    {
        timer = maxTime;
    }

    public bool IsPointing()
    {
        return timerActive;
    }

    IEnumerator PointToTarget()
    {
        while (enabled)
        {
            if (target)
                targetLocation = target.position;

            Vector3 relativePos = targetLocation - player.position;
            rotationToTarget = Quaternion.LookRotation(relativePos);
            rotationToTarget.z = -rotationToTarget.y;
            rotationToTarget.x = 0;
            rotationToTarget.y = 0;

            float relativeModifier = player.eulerAngles.y;
            indicatorTransform.localRotation = rotationToTarget * Quaternion.Euler(0, 0, relativeModifier);

            yield return null;
        }
    }

    IEnumerator StartTimer()
    {
        // Maybe do an animation here?
        indicatorCanvasGroup.alpha = 1.0f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        // Hide Indicator (later through animation
        indicatorCanvasGroup.alpha = 0.0f;
        endIndication(this);
        removeFromDict(target);
    }
}
