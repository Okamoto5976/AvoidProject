using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BGM,
    SE,
    Master
}

public class AudioManager : MonoBehaviour
{
    //音量はAudioMixerで制御する
    [SerializeField] private AudioMixer m_mixer;

    //BGM
    [SerializeField] private string m_bgmParam = "BGMVolume";
    [SerializeField] private AudioSource m_bgmSource;

    //SE
    [SerializeField] private string m_seParam = "SEVolume";
    [SerializeField] private AudioSource m_seSource;
    [SerializeField] private int m_maxSESources = 16;

    //MasterVolume
    [SerializeField] private string m_masterParam = "MasterVolume";

    private Coroutine m_bgmFadeCoroutine;

    //音量設定
    private const float MinDecibel = -50f;

    private Dictionary<SoundType, string> parameters;

    //同時に鳴らすSE用のAudioSource（m_seSourceを1つ目として使う）
    private readonly List<AudioSource> m_sePool = new List<AudioSource>();

    private void Awake()
    {
        parameters = new Dictionary<SoundType, string>
        {
            { SoundType.BGM, m_bgmParam },
            { SoundType.SE,  m_seParam  },
            { SoundType.Master, m_masterParam }
        };

        if (m_seSource != null)
        {
            m_sePool.Add(m_seSource);
        }
    }

    public void SetVolume(SoundType type, float volume)
    {
        float clamped = Mathf.Clamp01(volume);

        if (m_mixer != null && parameters.TryGetValue(type, out var param))
        {
            m_mixer.SetFloat(param, ToDecibel(clamped));
        }
    }

    //現在の音量を返す
    public float GetVolume(SoundType type)
    {
        if (m_mixer != null
            && parameters.TryGetValue(type, out var param)
            && m_mixer.GetFloat(param, out float decibel))
        {
            return ToVolume(decibel);
        }
        return 0f;
    }

    public void DecreaseVolume(SoundType type, float amount)
    {
        SetVolume(type, GetVolume(type) - amount);
    }

    //SEを再生する（volume/pitch/loopはAudioDataの設定を使う）
    public void PlaySE(AudioData data)
    {
        if (data == null || data.clip == null) return;

        AudioSource source = GetFreeSESource();
        if (source == null) return;

        source.clip = data.clip;
        source.volume = Mathf.Clamp01(data.volume);
        source.pitch = data.pitch;
        source.loop = data.loop;
        source.Play();
    }

    //空いているSE用AudioSourceを返す。無ければ上限まで追加する
    private AudioSource GetFreeSESource()
    {
        foreach (var source in m_sePool)
        {
            if (source != null && !source.isPlaying) return source;
        }

        if (m_seSource == null || m_sePool.Count >= m_maxSESources) return null;

        var added = gameObject.AddComponent<AudioSource>();
        added.playOnAwake = false;
        added.outputAudioMixerGroup = m_seSource.outputAudioMixerGroup;
        m_sePool.Add(added);
        return added;
    }

    //BGMのフェードアウトから次のBGMのフェードイン
    public void ChangeBGM(AudioClip nextClip, float fadeTime = 1f)
    {
        if (m_bgmSource == null) return;

        if (nextClip != null && m_bgmSource.clip == nextClip && m_bgmSource.isPlaying) return;

        CancelBGMFade();
        m_bgmFadeCoroutine = StartCoroutine(ChangeBGMRoutine(nextClip, fadeTime));
    }

    //BGMのフェードアウト
    public void FadeOutBGM(float fadeTime = 1f)
    {
        ChangeBGM(null, fadeTime);
    }

    //音源を止める
    public void Stop(SoundType type)
    {
        switch (type)
        {
            case SoundType.Master:
                StopAll();
                break;

            case SoundType.BGM:
                CancelBGMFade();
                StopSource(m_bgmSource);
                break;

            case SoundType.SE:
                foreach (var source in m_sePool)
                {
                    StopSource(source);
                }
                break;
        }
    }

    //全部の音源を止める
    public void StopAll()
    {
        Stop(SoundType.BGM);
        Stop(SoundType.SE);
    }

    private static void StopSource(AudioSource source)
    {
        if (source == null) return;

        source.Stop();
        source.volume = 1f;
        source.pitch = 1f;
        source.loop = false;
    }

    private void CancelBGMFade()
    {
        if (m_bgmFadeCoroutine == null) return;

        StopCoroutine(m_bgmFadeCoroutine);
        m_bgmFadeCoroutine = null;
    }

    private IEnumerator ChangeBGMRoutine(AudioClip nextClip, float fadeTime)
    {
        if (m_bgmSource.isPlaying)
        {
            yield return FadeBGM(m_bgmSource.volume, 0f, fadeTime);
        }
        m_bgmSource.Stop();

        if (nextClip != null)
        {
            m_bgmSource.clip = nextClip;
            m_bgmSource.volume = 0f;
            m_bgmSource.Play();
            yield return FadeBGM(0f, 1f, fadeTime);
        }

        m_bgmFadeCoroutine = null;
    }

    private IEnumerator FadeBGM(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            m_bgmSource.volume = to;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            m_bgmSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        m_bgmSource.volume = to;
    }

    private static float ToDecibel(float volume)
    {
        if (volume <= 0.0001f) return MinDecibel;
        return Mathf.Max(MinDecibel, Mathf.Log10(volume) * 20f);
    }

    private static float ToVolume(float decibel)
    {
        if (decibel <= MinDecibel) return 0f;
        return Mathf.Clamp01(Mathf.Pow(10f, decibel / 20f));
    }
}