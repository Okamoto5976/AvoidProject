using UnityEngine;
using UnityEngine.UI;

public class MenuTextUI : MonoBehaviour
{
    [SerializeField] private Button m_button;

    [SerializeField] private Animator m_anim;

    public Button Button => m_button;

    public void SetAnim(bool value)
    {
        if (!gameObject.activeInHierarchy) return;

        m_anim.SetBool("Selected", value);
    }
}
