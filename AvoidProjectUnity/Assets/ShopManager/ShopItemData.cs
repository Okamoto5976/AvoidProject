using System.Collections.Generic;
using UnityEngine;

/// <summary>商品1つ分の情報。ID・名前・価格を持つ。</summary>
[System.Serializable]
public class ShopItem
{
    [SerializeField] private int m_Id;
    [SerializeField] private string m_ItemName;
    [SerializeField] private int m_Price;

    public int Id => m_Id;
    public string ItemName => m_ItemName;
    public int Price => m_Price;
}

/// <summary>ショップで売る商品をまとめて持つData。商品はListで複数登録できる。</summary>
[CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    [SerializeField] private List<ShopItem> m_Items = new List<ShopItem>();

    public IReadOnlyList<ShopItem> Items => m_Items;
}