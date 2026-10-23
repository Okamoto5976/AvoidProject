using UnityEngine;
using UnityEngine.UI;

public class HeartView : MonoBehaviour
{
    [SerializeField] private Image m_fillImage;

    protected virtual void Awake()
    {
        if (m_fillImage != null && m_fillImage.type != Image.Type.Filled)
        {
            Debug.LogWarning("HeartView: Fill Image‚ÌImage Type‚ðFilled‚É‚µ‚Ä‚­‚¾‚³‚¢", this);
        }
    }

    public virtual void SetFill(float ratio)
    {
        if (m_fillImage == null) return;
        m_fillImage.fillAmount = Mathf.Clamp01(ratio);
    }
}