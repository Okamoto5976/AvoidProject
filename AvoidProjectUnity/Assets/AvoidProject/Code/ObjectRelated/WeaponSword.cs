using UnityEngine;

public class WeaponSword : MonoBehaviour
{
    private StageManager m_stageManager;

    public void Initialize(StageManager stageManager)
    {
        m_stageManager = stageManager;
    }
}
