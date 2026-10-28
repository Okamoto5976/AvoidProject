using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleMediationPool m_MediationPool;

    /// <summary>
    /// パーティクルを生成する関数
    /// </summary>
    /// <param name="type">パーティクルの種類</param>
    public void ActiveParticle(ParticleType type, Vector2 pos, float duration)
    {
        PoolManager pool = m_MediationPool.GetParticlePool(type);
        GameObject particle = pool.GenerateFromPool();
        particle.transform.position = pos;

        ParticleObject particleObject = particle.GetComponent<ParticleObject>();
        particleObject.Initialize(duration);
    }
}