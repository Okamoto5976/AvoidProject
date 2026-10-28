using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleMediationPool m_MediationPool;

    /// <summary>
    /// パーティクルを生成する関数
    /// </summary>
    /// <param name="type">パーティクルの種類</param>
    public void ActiveParticle(ParticleType type, float duration)
    {
        PoolManager pool = m_MediationPool.GetParticlePool(type);
        GameObject particle = pool.GenerateFromPool();

        ReturnPool returnPool;
        if (!particle.TryGetComponent<ReturnPool>(out returnPool))
        {
            Debug.Log("ReturnPoolがなかったため追加しました");
            returnPool = particle.AddComponent<ReturnPool>();
        }

        returnPool.SetReturnPool(pool);
        ParticleObject particleObject = particle.GetComponent<ParticleObject>();
        particleObject.Initialize(duration);
    }
}