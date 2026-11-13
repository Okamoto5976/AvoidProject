using UnityEngine;

public class OnStageSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject m_StyleTab;
    [SerializeField] private GameObject m_EnemyTab;

    private StageSelect m_stageSelect;
    private RuntimeData m_runtimeData;

    private void Awake()
    {
        m_runtimeData = new();
        m_stageSelect = new(m_runtimeData);
    }

    /// <summary>
    /// ボタンから戦闘方式をセットする関数
    /// </summary>
    /// <param name="isLimit">制限時間を設ける方式かどうか</param>
    public void OnSetBattleStyle(bool isLimit)
    {
        m_stageSelect.SetBattleStyle(isLimit);
        m_StyleTab.SetActive(false);
        m_EnemyTab.SetActive(true);
    }

    /// <summary>
    /// ボタンから敵を設定する関数
    /// </summary>
    /// <param name="id">セットする敵のID</param>
    public void OnSetEnemy(int id)
    {
        m_stageSelect.SetEnemyId(id);
        m_EnemyTab.SetActive(false);
    }

    /// <summary>
    /// 正しく設定できているかを確認する関数　ボタンから呼ぶ
    /// </summary>
    public void DebugData()
    {
        Debug.Log($"RuntimeData.HasTimeLimit = {m_runtimeData.HasTimeLimit}, " +
            $"Enemy = {GetEnemy(m_stageSelect.EnemyId)}");
    }
    //これもデバッグ用
    private string GetEnemy(int id)
    {
        switch (id)
        {
            case 0:
                return "イノシシ";
            case 1:
                return "ゴブリン";
            case 2:
                return "ゴーレム";
        }
        return null;
    }
}
