using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioManager m_audioManager;

    [Header("çƒê∂ÇÃÉeÉXÉg")]
    [SerializeField] private AudioData m_testBGM;
    [SerializeField] private AudioData m_nextBGM;
    [SerializeField] private AudioData m_testSE;
    [SerializeField] private float m_fadeTime = 1f;

    private void Update()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.aKey.wasPressedThisFrame) m_audioManager.PlayBGM(m_testBGM, m_fadeTime);
        if (Keyboard.current.mKey.wasPressedThisFrame) m_audioManager.PlayBGM(m_nextBGM, m_fadeTime);
        if (Keyboard.current.sKey.wasPressedThisFrame) m_audioManager.PlaySE(m_testSE);
        if (Keyboard.current.fKey.wasPressedThisFrame) m_audioManager.FadeOutBGM(m_fadeTime);
        if (Keyboard.current.xKey.wasPressedThisFrame) m_audioManager.Stop(SoundType.BGM);
        if (Keyboard.current.cKey.wasPressedThisFrame) m_audioManager.Stop(SoundType.SE);
        if (Keyboard.current.zKey.wasPressedThisFrame) m_audioManager.StopAll();
    }
}