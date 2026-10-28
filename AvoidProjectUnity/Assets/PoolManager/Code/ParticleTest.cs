using UnityEngine;
using UnityEngine.InputSystem;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] private ParticleManager m_ParticleManager;
    [SerializeField] private float particleDuration;

    private void Update()
    {
        //test
        if (Keyboard.current.f1Key.wasPressedThisFrame && m_ParticleManager != null)
        {
            m_ParticleManager.ActiveParticle(ParticleType.Type1, Vector2.zero, particleDuration);
        }
        if (Keyboard.current.f2Key.wasPressedThisFrame && m_ParticleManager != null)
        {
            m_ParticleManager.ActiveParticle(ParticleType.Type2, Vector2.zero, particleDuration);
        }
        if (Keyboard.current.f3Key.wasPressedThisFrame && m_ParticleManager != null)
        {
            m_ParticleManager.ActiveParticle(ParticleType.Type3, Vector2.zero, particleDuration);
        }
    }
}