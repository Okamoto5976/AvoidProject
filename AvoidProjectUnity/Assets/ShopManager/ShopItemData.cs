using UnityEngine;

/// <summary>商品1つ分のData。ID・名前・価格を持つ。</summary>
//これテストとして作ったので消していいです！！
[CreateAssetMenu(fileName = "ShopItemData", menuName = "Shop/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    [SerializeField] private int m_Id;
    [SerializeField] private string m_ItemName;
    [SerializeField] private int m_Price;

    public int Id => m_Id;
    public string ItemName => m_ItemName;
    public int Price => m_Price;
}