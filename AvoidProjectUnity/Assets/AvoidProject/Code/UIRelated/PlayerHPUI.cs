using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private GameObject m_redheart;
    [SerializeField] private GameObject m_greenheart;

    public void Initialize()
    {
        m_redheart.SetActive(true);
        m_greenheart.SetActive(false);
    }

    public void ViewRedHeart(bool value)
    {
        m_redheart.SetActive(value);
    }

    public void ViewGreenHeart(bool value)
    {
        m_greenheart.SetActive(value);
    }
}
