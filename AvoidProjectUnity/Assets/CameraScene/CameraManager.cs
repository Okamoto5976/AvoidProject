using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    //追従
    [SerializeField] private float m_followSmoothTime = 0.15f;

    //ズーム
    [SerializeField] private float m_minSize = 5f;
    [SerializeField] private float m_maxSize = 12f;

    //映したいオブジェクトの外側の余白
    [SerializeField] private float m_padding = 2f;

    //ズームの滑らかさ(小さい値程素早くズーム)
    [SerializeField] private float m_zoomSmoothTime = 0.25f;

    private Camera m_camera;

    //カメラに映したいオブジェクト(2つ以上でもOK)
    private readonly List<GameObject> m_targets = new List<GameObject>();

    //追従とズームを行っているか(Stopでfalseになる)
    private bool m_isTracking;

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


    //-----初期化(InitializedManagerから呼ばれる)-------

    /// <summary>
    /// PlayerとEnemyを渡して追従を開始する。
    /// 開始時はカメラを滑らせず、一瞬で正しい位置とサイズに合わせる。
    /// </summary>
    public void Initialize(GameObject player, GameObject enemy)
    {
        StartTracking(player, enemy);

        if (m_camera == null) m_camera = GetComponent<Camera>();
        if (TryGetBounds(out Bounds bounds))
        {
            m_basePosition = new Vector3(bounds.center.x, bounds.center.y, transform.position.z);
            transform.position = m_basePosition;
            m_camera.orthographicSize = CalcTargetSize(bounds);
        }
    }


    //-----追従の開始と停止-------

    /// <summary>
    /// 渡されたオブジェクトが全て映るように、カメラが動きながら追従とズームを行う。
    /// 呼び直すと映す対象を入れ替える(Stop中に呼ぶと再開する)。
    /// </summary>
    public void StartTracking(params GameObject[] targets)
    {
        m_targets.Clear();

        if (targets != null)
        {
            foreach (GameObject target in targets)
            {
                if (target != null) m_targets.Add(target);
            }
        }

        m_followVelocity = Vector3.zero;
        m_zoomVelocity = 0f;
        m_isTracking = true;
    }

    /// <summary>
    /// 追従とズームを止める。カメラはその位置とサイズで止まる(シェイクは動く)。
    /// </summary>
    public void Stop()
    {
        m_isTracking = false;
        m_followVelocity = Vector3.zero;
        m_zoomVelocity = 0f;
    }

    private void LateUpdate()
    {
        if (m_isTracking && TryGetBounds(out Bounds bounds))
        {
            Vector3 goal = new Vector3(bounds.center.x, bounds.center.y, m_basePosition.z);
            m_basePosition = Vector3.SmoothDamp(m_basePosition, goal, ref m_followVelocity, m_followSmoothTime);

            m_camera.orthographicSize = Mathf.SmoothDamp(
                m_camera.orthographicSize, CalcTargetSize(bounds), ref m_zoomVelocity, m_zoomSmoothTime);
        }

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

    //映したいオブジェクト全てを囲む範囲を取得(Destroyされたものは除く)
    private bool TryGetBounds(out Bounds bounds)
    {
        bool found = false;
        bounds = new Bounds();

        foreach (GameObject target in m_targets)
        {
            if (target == null) continue;

            Vector3 position = target.transform.position;
            if (!found)
            {
                bounds = new Bounds(position, Vector3.zero);
                found = true;
            }
            else
            {
                bounds.Encapsulate(position);
            }
        }

        return found;
    }

    //全てのオブジェクトが収まるのに必要なカメラのサイズの計算
    private float CalcTargetSize(Bounds bounds)
    {
        float halfWidth = bounds.extents.x + m_padding;
        float halfHeight = bounds.extents.y + m_padding;

        float sizeByWidth = halfWidth / m_camera.aspect;
        float size = Mathf.Max(halfHeight, sizeByWidth);

        return Mathf.Clamp(size, m_minSize, m_maxSize);
    }
}