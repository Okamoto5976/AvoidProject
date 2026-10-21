using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private AudioSource m_bgmSource;

    //SE
    [SerializeField] private string m_seParam = "SEVolume";
    [SerializeField] private Slider m_seSlider;
    [SerializeField] private AudioSource m_seSource;

    //MasterVolume
    [SerializeField] private string m_masterParam = "MasterVolume";
    [SerializeField] private Slider m_masterSlider;


    private Coroutine m_bgmFadeCoroutine;

    //音量設定
    private const float MinDecibel = -50f;

    private Dictionary<SoundType, string> parameters;
    private Dictionary<SoundType, Slider> sliders;
    private Dictionary<SoundType, AudioSource> sources;

    private void Awake()
    {
        sources = new Dictionary<SoundType, AudioSource>
        {
            { SoundType.BGM, m_bgmSource },
            { SoundType.SE,  m_seSource  }
        };

        parameters = new Dictionary<SoundType, string>
        {
            { SoundType.BGM, m_bgmParam },
            { SoundType.SE,  m_seParam  },
            { SoundType.Master, m_masterParam }
        };

        sliders = new Dictionary<SoundType, Slider>
        {
            { SoundType.BGM, m_bgmSlider },
            { SoundType.SE,  m_seSlider  },
            { SoundType.Master, m_masterSlider }
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
        if (type == SoundType.Master)
        {
            StopAll();
            return;
        }

        if (type == SoundType.BGM)
        {
            CancelBGMFade();
        }

        if (sources.TryGetValue(type, out var source) && source != null)
        {
            source.Stop();
            source.volume = 1f;
        }
    }

    //全部の音源を止める
    public void StopAll()
    {
        foreach (var type in sources.Keys)
        {
            Stop(type);
        }
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