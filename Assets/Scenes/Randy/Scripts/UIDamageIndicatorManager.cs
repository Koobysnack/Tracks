using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageIndicatorManager : MonoBehaviour
{
    public static UIDamageIndicatorManager instance;
    [SerializeField] Transform indicatorParent;
    [SerializeField] float hitDuration;
    [SerializeField] bool showOnScreenHit;
    [SerializeField] bool showOnScreenWarning;
    [SerializeField] Indicator IndicatorPrefab;
    [SerializeField] Color hitColor;
    [SerializeField] Color warningColor;
    [SerializeField] HealthVFXController hpVFX;
    
    Transform player;
    ObjectPool<Indicator> indicatorPool;
    Dictionary<Transform, Indicator> indicators = new Dictionary<Transform, Indicator>();
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        indicatorPool = new ObjectPool<Indicator>(() => {   // Create
            Indicator newIndicator = Instantiate(IndicatorPrefab, indicatorParent);
            newIndicator.LateInit(ReleaseIndicator, ClearIndicator);
            return newIndicator;
        }, indicator => {   // Get
            indicator.gameObject.SetActive(true);
        }, indicator => {   // Release
            indicator.gameObject.SetActive(false);
        }, indicator => {   // Destroy
            Destroy(indicator);
        }, false, 10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance != null && player == null)
            player = GameManager.instance.player;
    }

    public void ReleaseIndicator(Indicator i)
    {
        indicatorPool.Release(i);
    }

    public void PlayerHit(Transform target)
    {
        hpVFX.SetIntensity(1 - player.gameObject.GetComponent<PlayerController>().GetHealthPercent());
        if (onScreenCheck(target.gameObject, showOnScreenHit))
            return;
        Indicator i;
        if (indicators.ContainsKey(target))
        {
            i = indicators[target];
            indicatorPool.Get(out i);
            if (i.IsPointing())
                i.ResetTimer();
            else
                i.RegisterHit(player, target, hitDuration, hitColor);

            return;
        }
        i = indicatorPool.Get();
        i.RegisterHit(player, target, hitDuration, hitColor);
        indicators.TryAdd(target, i);
    }

    public void PlayerWarn(Transform target, float duration)
    {
        if (onScreenCheck(target.gameObject, showOnScreenHit))
            return;
        Indicator i;
        if (indicators.ContainsKey(target))
        {
            i = indicators[target];
            indicatorPool.Get(out i);
            if (i.IsPointing())
                i.ResetTimer();
            else
                i.RegisterHit(player, target, duration, warningColor);

            return;
        }
        i = indicatorPool.Get();
        i.RegisterHit(player, target, duration, warningColor);
        indicators.TryAdd(target, i);
    }

    public void ClearIndicator(Transform target)
    {
        indicators.Remove(target);
    }

    bool onScreenCheck(GameObject target, bool screenCondtion)
    {
        bool visible = (target.GetComponentInChildren<Renderer>().isVisible);
        return (!visible || !screenCondtion) && visible;
    }
}
