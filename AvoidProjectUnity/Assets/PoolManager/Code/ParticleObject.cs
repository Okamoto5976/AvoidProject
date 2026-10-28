using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    private ReturnPool m_ReturnPool;
    private float m_timer;

    /// <summary>
    /// パーティクルの初期化関数
    /// </summary>
    /// <param name="duration">パーティクルの継続時間</param>
    public void Initialize(float duration)
    {
        m_ReturnPool = GetComponent<ReturnPool>();
        m_timer = duration;
    }

    private void Update()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            m_ReturnPool.Return();
        }
    }
}