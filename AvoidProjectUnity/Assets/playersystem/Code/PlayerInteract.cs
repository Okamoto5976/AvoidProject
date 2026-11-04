using UnityEngine;

public class PlayerInteract
{
    private IInteractable m_target;
    private Transform m_playerTransform;
    private float m_holdTimer = 0f;
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
                m_holdTimer = 0f;
                m_isCompleted = false;

                Debug.Log("Œ•‚ð”­Œ©I’·‰Ÿ‚µ‚µ‚Ä‚­‚¾‚³‚¢");
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

        m_holdTimer += deltaTime;

        if (m_holdTimer >= requiredHoldTime)
        {
            m_isCompleted = true;

            bool result = m_target.Interact(controller);

            if (result)
            {
                m_isCompleted = true;
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