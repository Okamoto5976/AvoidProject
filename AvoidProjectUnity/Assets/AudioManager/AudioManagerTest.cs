using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioManager m_audioManager;
    [SerializeField] private float m_step = 0.1f;

    [Header("BGM切り替えのテスト")]
    [SerializeField] private AudioData m_nextBGM;
    [SerializeField] private float m_fadeTime = 1f;

    [Header("SE再生のテスト")]
    [SerializeField] private AudioData m_testSE;

    private void Update()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.bKey.wasPressedThisFrame) DecreaseVolume(SoundType.BGM);
        if (Keyboard.current.nKey.wasPressedThisFrame) DecreaseVolume(SoundType.SE);
        if (Keyboard.current.vKey.wasPressedThisFrame) DecreaseVolume(SoundType.Master);
        if (Keyboard.current.xKey.wasPressedThisFrame) m_audioManager.Stop(SoundType.BGM);
        if (Keyboard.current.cKey.wasPressedThisFrame) m_audioManager.Stop(SoundType.SE);
        if (Keyboard.current.zKey.wasPressedThisFrame) m_audioManager.StopAll();
        if (Keyboard.current.mKey.wasPressedThisFrame) m_audioManager.ChangeBGM(m_nextBGM, m_fadeTime);
        if (Keyboard.current.sKey.wasPressedThisFrame) m_audioManager.PlaySE(m_testSE);
    }

    private void DecreaseVolume(SoundType type)
    {
        m_audioManager.DecreaseVolume(type, m_step);
        Debug.Log($"{type} volume: {m_audioManager.GetVolume(type):F1}");
    }
}