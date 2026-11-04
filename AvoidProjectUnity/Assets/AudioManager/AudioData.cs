using UnityEngine;

//再生する音の設定をまとめたScriptableObject
//Projectビューで右クリック > Create > Audio > AudioData で作成する
[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private AudioClip m_clip;
    [SerializeField, Range(0f, 1f)] private float m_volume = 1f;
    [SerializeField, Range(0.1f, 3f)] private float m_pitch = 1f;
    [SerializeField] private bool m_loop = false;

    public AudioClip Clip => m_clip;
    public float Volume => m_volume;
    public float Pitch => m_pitch;
    public bool Loop => m_loop;
}