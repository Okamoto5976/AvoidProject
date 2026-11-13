using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ShopManagerのテスト用。Panel内のButtonにIDを割り当てて、押したらIDを渡す(仮)。
/// 本体のShopManagerにはテスト処理を入れない。
/// </summary>
public class ShopTest : MonoBehaviour
{
    [System.Serializable]
    private class ItemButton
    {
        public Button m_Button;
        public int m_Id;
    }

    [SerializeField] private ShopManager m_ShopManager;
    [SerializeField] private ItemButton[] m_ItemButtons;

    private void Start()
    {
        foreach (ItemButton itemButton in m_ItemButtons)
        {
            // ラムダ内でループ変数を使うため、ローカルにコピーしておく
            int id = itemButton.m_Id;
            itemButton.m_Button.onClick.AddListener(() => OnClickBuy(id));
        }
    }

    private void OnClickBuy(int id)
    {
        PurchaseResult result = m_ShopManager.Purchase(id);

        switch (result)
        {
            case PurchaseResult.Success:
                Debug.Log($"[ShopTest] ID {id}: 購入成功");
                break;
            case PurchaseResult.NotEnoughMoney:
                Debug.Log($"[ShopTest] ID {id}: お金が足りません");
                break;
            case PurchaseResult.InvalidId:
                Debug.Log($"[ShopTest] ID {id}: 存在しない商品です");
                break;
        }
    }
}