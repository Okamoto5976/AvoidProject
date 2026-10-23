using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColliderPool
{
    public ShapeType Type;
    public PoolManager PoolManager;
}

public class ColliderMediationPool : MonoBehaviour
{
    [SerializeField] private List<ColliderPool> m_ColliderPools;

    /// <summary>
    /// コライダーのプールを取得する関数
    /// </summary>
    /// <param name="type">コライダーの形</param>
    /// <returns>コライダーのプール</returns>
    public PoolManager GetColliderPool(ShapeType type)
    {
        foreach (ColliderPool pool in m_ColliderPools)
        {
            if (pool.Type == type) return pool.PoolManager;
        }
        return null;
    }
}