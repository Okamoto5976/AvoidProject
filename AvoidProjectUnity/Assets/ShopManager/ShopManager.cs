using System.Collections.Generic;
using UnityEngine;

/// <summary>購入結果。UI側で結果ごとに表示を変えられるようにする。</summary>
public enum PurchaseResult
{
    Success,          // 購入成功
    NotEnoughMoney,   // お金が足りない
    InvalidId,        // 該当するIDの商品がない
    AlreadyPurchased, // 購入済み(ポーション以外は1回のみ)
}

/// <summary>
/// ショップの購入処理。
/// ボタンからIDを受け取り、Dataの所持金と商品価格で購入可否を判定する。
/// ID1(ポーション)は何度でも購入可能。それ以外は1回のみ。
/// </summary>
public class ShopManager : MonoBehaviour
{
    /// <summary>何度でも購入できるポーションのID。</summary>
    public const int PotionId = 1;

    [SerializeField] private ShopMoneyData m_MoneyData;
    [SerializeField] private ShopItemData m_ItemData;

    private ShopRuntimeData m_ShopRuntimeData;
    private Dictionary<int, ShopItem> m_ItemDict;

    /// <summary>ShopRuntimeDataを外から渡す。</summary>
    public void Initialize(ShopRuntimeData data)
    {
        m_ShopRuntimeData = data;
    }

    private void Awake()
    {
        // まだ外からShopRuntimeDataが渡されていない間は、仮で自前で作っておく
        if (m_ShopRuntimeData == null)
        {
            m_ShopRuntimeData = new ShopRuntimeData();
        }

        // IDから商品を引けるようにしておく
        m_ItemDict = new Dictionary<int, ShopItem>();
        foreach (ShopItem item in m_ItemData.Items)
        {
            if (item == null) continue;
            if (m_ItemDict.ContainsKey(item.Id))
            {
                Debug.LogWarning($"[Shop] ID {item.Id} が重複しています: {item.ItemName}");
                continue;
            }
            m_ItemDict.Add(item.Id, item);
        }
    }

    /// <summary>購入済みかどうか。ポーションは常に false(何度でも買える)。</summary>
    public bool IsPurchased(int id)
    {
        return m_ShopRuntimeData.m_PurchasedItemIds.Contains(id);
    }

    /// <summary>
    /// 購入ボタンから呼ぶ。渡されたIDの商品を、所持金から購入する。
    /// </summary>
    public PurchaseResult Purchase(int id)
    {
        if (!m_ItemDict.TryGetValue(id, out ShopItem item))
        {
            Debug.LogWarning($"[Shop] ID {id} の商品が見つかりません");
            return PurchaseResult.InvalidId;
        }

        // ポーション以外は購入済みなら処理をやめる
        if (id != PotionId && IsPurchased(id))
        {
            Debug.Log($"[Shop] 購入済みのため購入できません (ID:{id})");
            return PurchaseResult.AlreadyPurchased;
        }

        // Dataから所持金を変数に渡す
        int money = m_MoneyData.GetMoney();

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

        // ポーション以外は購入済みListに記録する
        if (id != PotionId)
        {
            m_ShopRuntimeData.m_PurchasedItemIds.Add(id);
        }

        Debug.Log($"[Shop] 購入しました (ID:{id} 価格:{price} 残金:{m_MoneyData.GetMoney()})");

        // TODO: ここでアイテム付与(インベントリ追加など)を行う
        // TODO: ポーションの個数制限は後で追加する
        return PurchaseResult.Success;
    }
}