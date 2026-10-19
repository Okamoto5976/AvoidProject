using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SoundType
{
    BGM,
    SE
}

public class AudioManager : MonoBehaviour
{
    //音量はAudioMixerで制御する
    [SerializeField] private AudioMixer m_mixer;

    [SerializeField] private string m_bgmParam = "BGMVolume";
    [SerializeField] private string m_seParam = "SEVolume";

    //音量調整用のスライダー
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;

    //BGMを鳴らすAudioSource
    [SerializeField] private AudioSource m_bgmSource;

    private Coroutine m_bgmFadeCoroutine;

    private const float MinDecibel = -80f;

    private Dictionary<SoundType, string> parameters;
    private Dictionary<SoundType, Slider> sliders;

    private void Awake()
    {
        parameters = new Dictionary<SoundType, string>
        {
            { SoundType.BGM, m_bgmParam },
            { SoundType.SE,  m_seParam  }
        };

        sliders = new Dictionary<SoundType, Slider>
        {
            { SoundType.BGM, m_bgmSlider },
            { SoundType.SE,  m_seSlider  }
        };
    }

    private void Start()
    {
        foreach (var pair in sliders)
        {
            BindSlider(pair.Key, pair.Value);
        }
    }

    private void BindSlider(SoundType type, Slider slider)
    {
        if (slider == null) return;

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.SetValueWithoutNotify(GetVolume(type));
        slider.onValueChanged.AddListener(value => SetVolume(type, value));
    }

    public void SetVolume(SoundType type, float volume)
    {
        float clamped = Mathf.Clamp01(volume);

        if (m_mixer != null && parameters.TryGetValue(type, out var param))
        {
            m_mixer.SetFloat(param, ToDecibel(clamped));
        }

        if (sliders.TryGetValue(type, out var slider) && slider != null)
        {
            slider.SetValueWithoutNotify(clamped);
        }
    }
    public float GetVolume(SoundType type)
    {
        if (m_mixer != null
            && parameters.TryGetValue(type, out var param)
            && m_mixer.GetFloat(param, out float decibel))
        {
            return ToVolume(decibel);
        }

        Debug.LogWarning($"AudioManager: {type} の音量を取得できません。MixerとExposeしたパラメータ名を確認してください", this);
        return 0f;
    }

    public void DecreaseVolume(SoundType type, float amount)
    {
        SetVolume(type, GetVolume(type) - amount);
    }

    public void ChangeBGM(AudioClip nextClip, float fadeTime = 1f)
    {
        if (m_bgmSource == null) return;

        if (nextClip != null && m_bgmSource.clip == nextClip && m_bgmSource.isPlaying) return;

        if (m_bgmFadeCoroutine != null)
        {
            StopCoroutine(m_bgmFadeCoroutine);
        }
        m_bgmFadeCoroutine = StartCoroutine(ChangeBGMRoutine(nextClip, fadeTime));
    }

    public void FadeOutBGM(float fadeTime = 1f)
    {
        ChangeBGM(null, fadeTime);
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