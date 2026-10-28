using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    //追従
    [SerializeField] private float m_followSmoothTime = 0.15f;

    //ズーム
    [SerializeField] private float m_minSize = 5f;
    [SerializeField] private float m_maxSize = 12f;

    //EnemyとPlayerの外側の余白
    [SerializeField] private float m_padding = 2f;

    //ズームの滑らかさ(小さい値程素早くズーム)
    [SerializeField] private float m_zoomSmoothTime = 0.25f;

    private Camera m_camera;
    private GameObject m_player;
    private GameObject m_enemy;

    private Vector3 m_basePosition;
    private Vector3 m_followVelocity;
    private float m_zoomVelocity;

    // シェイク用
    private float m_shakeDuration;
    private float m_shakeElapsed;
    private float m_shakeMagnitude;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_basePosition = transform.position;
    }


    //-----PlayerとEnemyの関数-------

    public void Initialize(GameObject player, GameObject enemy)
    {
        m_player = player;
        m_enemy = enemy;

        if (m_camera == null) m_camera = GetComponent<Camera>();
        if (TryGetTargetPosition(out Vector3 pos))
        {
            m_basePosition = new Vector3(pos.x, pos.y, transform.position.z);
            transform.position = m_basePosition;
        }
        m_camera.orthographicSize = CalcTargetSize();
        m_followVelocity = Vector3.zero;
        m_zoomVelocity = 0f;
    }

    private void LateUpdate()
    {
        if (TryGetTargetPosition(out Vector3 target))
        {
            Vector3 goal = new Vector3(target.x, target.y, m_basePosition.z);
            m_basePosition = Vector3.SmoothDamp(m_basePosition, goal, ref m_followVelocity, m_followSmoothTime);
        }

        m_camera.orthographicSize = Mathf.SmoothDamp(
            m_camera.orthographicSize, CalcTargetSize(), ref m_zoomVelocity, m_zoomSmoothTime);

        transform.position = m_basePosition + CalcShakeOffset();
    }

    //カメラの揺れ
    public void Shake(float duration, float magnitude)
    {
        if (m_shakeElapsed < m_shakeDuration && magnitude < CurrentShakeMagnitude()) return;

        m_shakeDuration = duration;
        m_shakeMagnitude = magnitude;
        m_shakeElapsed = 0f;
    }

    private float CurrentShakeMagnitude()
    {
        if (m_shakeDuration <= 0f) return 0f;
        return m_shakeMagnitude * (1f - m_shakeElapsed / m_shakeDuration);
    }

    private Vector3 CalcShakeOffset()
    {
        if (m_shakeElapsed >= m_shakeDuration) return Vector3.zero;

        m_shakeElapsed += Time.deltaTime;
        Vector2 offset = Random.insideUnitCircle * CurrentShakeMagnitude();
        return new Vector3(offset.x, offset.y, 0f);
    }
    private bool TryGetTargetPosition(out Vector3 position)
    {
        bool hasPlayer = m_player != null;
        bool hasEnemy = m_enemy != null;

        if (hasPlayer && hasEnemy)
        {
            position = (m_player.transform.position + m_enemy.transform.position) * 0.5f;
            return true;
        }
        if (hasPlayer) { position = m_player.transform.position; return true; }
        if (hasEnemy) { position = m_enemy.transform.position; return true; }

        position = Vector3.zero;
        return false;
    }

    //PlayerとEnemyが収まるのに必要なカメラのサイズの計算
    private float CalcTargetSize()
    {
        if (m_player == null || m_enemy == null) return m_minSize;

        Vector3 diff = m_player.transform.position - m_enemy.transform.position;
        float halfWidth = Mathf.Abs(diff.x) * 0.5f + m_padding;
        float halfHeight = Mathf.Abs(diff.y) * 0.5f + m_padding;

        float sizeByWidth = halfWidth / m_camera.aspect;
        float size = Mathf.Max(halfHeight, sizeByWidth);

        return Mathf.Clamp(size, m_minSize, m_maxSize);
    }
}