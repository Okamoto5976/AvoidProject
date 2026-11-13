using UnityEngine;

/// <summary>
/// ショップ用の所持金Data(仮)。既存のPlayerDataとは別物。
/// 後で既存のDataと繋ぐときは、GetMoney / SpendMoney の中身だけ差し替えればよい。
/// </summary>
//テストとして作ったので消していいです！ 

public class ShopMoneyData : MonoBehaviour
{
    [SerializeField] private int m_Money = 1000;

    public int GetMoney()
    {
        return m_Money;
    }

    public void AddMoney(int amount)
    {
        m_Money += amount;
    }

    /// <summary>お金を減らす。足りなければ false を返して何もしない。</summary>
    public bool SpendMoney(int amount)
    {
        if (amount < 0 || m_Money < amount) return false;
        m_Money -= amount;
        return true;
    }
}