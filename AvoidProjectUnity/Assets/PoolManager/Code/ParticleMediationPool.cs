using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParticlePool
{
    public ParticleType Type;
    public PoolManager PoolManager;
}

public class ParticleMediationPool : MonoBehaviour
{
    [SerializeField] private List<ParticlePool> m_ParticlePools;

    /// <summary>
    /// エフェクトのプールを取得する関数
    /// </summary>
    /// <param name="type">エフェクトの種類</param>
    /// <returns>エフェクトのプール</returns>
    public PoolManager GetParticlePool(ParticleType type)
    {
        foreach (ParticlePool pool in m_ParticlePools)
        {
            if (pool.Type == type) return pool.PoolManager;
        }
        return null;
    }
}