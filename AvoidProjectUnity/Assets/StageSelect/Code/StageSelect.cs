public class StageSelect
{
    private RuntimeData m_RuntimeData;

    //EnemyIdを格納する場所がRuntimeDataにないので仮の格納場所
    public int EnemyId;

    /// <summary>
    /// StageSelectの初期化関数
    /// </summary>
    /// <param name="data">ランタイムデータ</param>
    public StageSelect(RuntimeData data)
    {
        m_RuntimeData = data;
    }

    /// <summary>
    /// 戦闘方式をセットする関数
    /// </summary>
    /// <param name="isLimit">制限時間を設ける方式かどうか</param>
    public void SetBattleStyle(bool isLimit)
    {
        m_RuntimeData.HasTimeLimit = isLimit;
    }

    /// <summary>
    /// 敵を設定する関数
    /// </summary>
    /// <param name="id">セットする敵のID</param>
    public void SetEnemyId(int id)
    {
        EnemyId = id;
    }
}