using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// CameraManagerの動作確認用スクリプト(テスト専用)。
/// 適当なGameObjectにアタッチして使う。
/// Spaceキー: 弱いシェイク / Enterキー: 強いシェイク
/// Sキー: 追従を止める(Stop) / Tキー: 追従を再開(StartTracking)
/// </summary>
public class CameraManagerTest : MonoBehaviour
{
    [SerializeField] private CameraManager m_cameraManager;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_enemy;

    private void Start()
    {
        m_cameraManager.StartTracking(m_player, m_enemy);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            m_cameraManager.Shake(0.2f, 0.2f);
        }
        if (keyboard.enterKey.wasPressedThisFrame)
        {
            m_cameraManager.Shake(0.5f, 0.6f);
        }
        if (keyboard.sKey.wasPressedThisFrame)
        {
            m_cameraManager.Stop();
        }
        if (keyboard.tKey.wasPressedThisFrame)
        {
            m_cameraManager.StartTracking(m_player, m_enemy);
        }
    }
}