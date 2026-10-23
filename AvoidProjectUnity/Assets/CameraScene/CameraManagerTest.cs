using UnityEngine;
using UnityEngine.InputSystem;
public class CameraManagerTest : MonoBehaviour
{
    [SerializeField] private CameraManager m_cameraManager;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_enemy;

    private void Start()
    {
        m_cameraManager.Initialized(m_player, m_enemy);
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
    }
}