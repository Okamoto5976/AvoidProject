using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class AudioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioManager m_audioManager;
    [SerializeField] private float m_step = 0.1f;

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return;
        if (Keyboard.current.bKey.wasPressedThisFrame) DecreaseVolume(SoundType.BGM);
        if (Keyboard.current.nKey.wasPressedThisFrame) DecreaseVolume(SoundType.SE);
#else
        if (Input.GetKeyDown(KeyCode.B)) DecreaseVolume(SoundType.BGM);
        if (Input.GetKeyDown(KeyCode.N)) DecreaseVolume(SoundType.SE);
#endif
    }

    private void DecreaseVolume(SoundType type)
    {
        m_audioManager.DecreaseVolume(type, m_step);
        Debug.Log($"{type} volume: {m_audioManager.GetVolume(type):F1}");
    }
}