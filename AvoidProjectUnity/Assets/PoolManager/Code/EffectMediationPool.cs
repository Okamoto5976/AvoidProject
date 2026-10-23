using System;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    None
}

[Serializable]
public class EffectPool
{
    public EffectType Type;
    public PoolManager PoolManager;
}

public class EffectMediationPool : MonoBehaviour
{
    [SerializeField] private List<EffectPool> m_EffectPools;

    /// <summary>
    /// エフェクトのプールを取得する関数
    /// </summary>
    /// <param name="type">エフェクトの種類</param>
    /// <returns>エフェクトのプール</returns>
    public PoolManager GetEffectPool(EffectType type)
    {
        foreach (EffectPool pool in m_EffectPools)
        {
            if (pool.Type == type) return pool.PoolManager;
        }
        return null;
    }
}