using System.Collections.Generic;

/// <summary>
/// ショップ用の実行中データ。購入済みアイテムのIDを持つ。
/// 既存のRuntimeDataを書き換えないために、別クラスにしている。
/// </summary>
[System.Serializable]
public class ShopRuntimeData
{
    // Purchase item which(bool)
    // 購入済みアイテムのID一覧(ID1のポーションは何度も買えるので入れない)
    public List<int> m_PurchasedItemIds = new List<int>();
}