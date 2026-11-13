using System.Collections.Generic;
using UnityEngine;

/// <summary>購入結果。UI側で結果ごとに表示を変えられるようにする。</summary>
public enum PurchaseResult
{
    Success,        // 購入成功
    NotEnoughMoney, // お金が足りない
    InvalidId,      // 該当するIDの商品がない
}

/// <summary>
/// ショップの購入処理。
/// ボタンからIDを受け取り、Dataの所持金と商品価格で購入可否を判定する。
/// </summary>
public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopMoneyData m_MoneyData;
    [SerializeField] private List<ShopItemData> m_ItemList = new List<ShopItemData>();

    private Dictionary<int, ShopItemData> m_ItemDict;

    private void Awake()
    {
        // IDから商品を引けるようにしておく
        m_ItemDict = new Dictionary<int, ShopItemData>();
        foreach (ShopItemData item in m_ItemList)
        {
            if (item == null) continue;
            if (m_ItemDict.ContainsKey(item.Id))
            {
                Debug.LogWarning($"[Shop] ID {item.Id} が重複しています: {item.name}");
                continue;
            }
            m_ItemDict.Add(item.Id, item);
        }
    }

    /// <summary>
    /// 購入ボタンから呼ぶ。渡されたIDの商品を、所持金から購入する。
    /// </summary>
    public PurchaseResult Purchase(int id)
    {
        // Dataから所持金を変数に渡す
        int money = m_MoneyData.GetMoney();

        if (!m_ItemDict.TryGetValue(id, out ShopItemData item))
        {
            Debug.LogWarning($"[Shop] ID {id} の商品が見つかりません");
            return PurchaseResult.InvalidId;
        }

        return Purchase(id, money, item.Price);
    }

    /// <summary>ID・所持金・価格から購入可否を判定して処理する。</summary>
    private PurchaseResult Purchase(int id, int money, int price)
    {
        if (money < price)
        {
            Debug.Log($"[Shop] お金が足りないため購入できません (ID:{id} 所持金:{money} 価格:{price})");
            return PurchaseResult.NotEnoughMoney;
        }

        m_MoneyData.SpendMoney(price);
        Debug.Log($"[Shop] 購入しました (ID:{id} 価格:{price} 残金:{m_MoneyData.GetMoney()})");

        // ここでアイテム付与(インベントリ追加など)を行う
        return PurchaseResult.Success;
    }
}