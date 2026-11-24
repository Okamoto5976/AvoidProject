using UnityEngine;

public class PlayerInteract
{
    private IInteractable m_target;
    private Transform m_playerTransform;
    private float m_holdTimer = 0f;
    private float m_interactRange;
    private bool m_isCompleted = false;

    public PlayerInteract(Transform playerTransform)
    {
        m_playerTransform = playerTransform;
    }

    public void StartInteract(PlayerController controller, float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            m_playerTransform.position,
            range
        );

        foreach (Collider2D col in colliders)
        {
            IInteractable interactable =
                col.GetComponent<IInteractable>();

            if (interactable != null)
            {
                m_target = interactable;
                m_interactRange = range;
                m_holdTimer = 0f;
                m_isCompleted = false;

                Debug.Log("剣を発見！長押ししてください");
                return;
            }
        }

        m_target = null;
    }

    public void Interact(
        PlayerController controller,
        float deltaTime,
        float requiredHoldTime)
    {
        if (m_target == null || m_isCompleted)
        {
            return;
        }

        // プレイヤーと剣の距離を確認
        float distance = Vector2.Distance(
            m_playerTransform.position,
            ((MonoBehaviour)m_target).transform.position
        );

        //範囲外ならインタラクトを中断
        if (distance > m_interactRange)
        {
            Debug.Log("剣から離れたため拾得を中断します。");
            EndInteract();
            return;
        }

        m_holdTimer += deltaTime;

        if (m_holdTimer >= requiredHoldTime)
        {
            m_isCompleted = true;

            bool result = m_target.Interact(controller);

            if (result)
            {
                m_target = null;
            }
            else
            {
                m_isCompleted = false;
            }
        }
    }

    public void EndInteract()
    {
        m_target = null;
        m_holdTimer = 0f;
        m_isCompleted = false;
    }
}