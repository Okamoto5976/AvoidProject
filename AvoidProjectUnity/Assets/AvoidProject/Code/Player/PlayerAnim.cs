using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator m_anim;

    private void Awake()
    {
        m_anim = GetComponentInChildren<Animator>();
    }

    public void IdleAnim(Vector2 dir)
    {
        m_anim.SetFloat("DirX", dir.x);
        m_anim.SetFloat("DirY", dir.y);
    }
}
