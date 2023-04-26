using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIDamageIndicatorManager : MonoBehaviour
{
    public static UIDamageIndicatorManager instance;
    [SerializeField] Transform indicatorParent;
    public float duration;
    Transform player;
    [SerializeField] Indicator IndicatorPrefab;
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
            return Instantiate(IndicatorPrefab, indicatorParent);
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
        Indicator i;
        //if (indicators.ContainsKey(target))
        //{
        //    i = indicators[target];
        //    indicatorPool.Get(out i);
        //    i.ResetTimer();
        //    return;
        //}
        i = indicatorPool.Get();
        i.RegisterHit(player, target, ReleaseIndicator, ClearIndicator);
        indicators.Add(target, i);
    }

    public void ClearIndicator(Transform target)
    {
        indicators.Clear();
    }
}
