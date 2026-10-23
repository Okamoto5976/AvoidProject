using UnityEngine;

public class ReturnPool : MonoBehaviour
{
    private PoolManager m_PoolManager;

    /// <summary>
    /// 戻るべきプールを設定する関数
    /// </summary>
    /// <param name="pool">戻るべきプール</param>
    public void SetReturnPool(PoolManager pool)
    {
        m_PoolManager = pool;
    }

    /// <summary>
    /// プールに戻らせる関数
    /// </summary>
    public void Return()
    {
        m_PoolManager.ReturnToPool(this.gameObject);
    }
}
