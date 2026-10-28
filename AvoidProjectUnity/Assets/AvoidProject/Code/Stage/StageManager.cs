using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Vector2 m_center;
    private float m_limitRadius;

    /// <summary>
    /// 初期化時に呼ぶ 
    /// </summary>
    /// <param name="radius"></param>
    public void Initialize(float radius)//後々引数が増えるようなら　別ごとに関数を作るか　Classで渡してあげる
    {
        m_limitRadius = radius;
    }

    public Vector2 LimitMovement(Vector2 pos)
    {
        Vector2 offset = pos - m_center;

        if(offset.magnitude > m_limitRadius)
        {
            pos = m_center + offset.normalized * m_limitRadius;
        }

        return pos;
    }

    //poolからオブジェクトを拾う？poolは使わず　この中で処理を行う
}
