using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    BGM,
    SE
}

public class AudioManager : MonoBehaviour
{
    //BGM
    [SerializeField] private AudioSource m_bgmSource;
    [SerializeField] private Slider m_bgmSlider;

    //SE
    [SerializeField] private AudioSource m_seSource;
    [SerializeField] private Slider m_seSlider;

    private Dictionary<SoundType, AudioSource> sources;
    private Dictionary<SoundType, Slider> sliders;

    private void Awake()
    {
        sources = new Dictionary<SoundType, AudioSource>
        {
            { SoundType.BGM, m_bgmSource },
            { SoundType.SE,  m_seSource  }
        };

        sliders = new Dictionary<SoundType, Slider>
        {
            { SoundType.BGM, m_bgmSlider },
            { SoundType.SE,  m_seSlider  }
        };

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

        if (sources.TryGetValue(type, out var source) && source != null)
        {
            source.volume = clamped;
        }

        if (sliders.TryGetValue(type, out var slider) && slider != null)
        {
            slider.SetValueWithoutNotify(clamped);
        }
    }

    public float GetVolume(SoundType type)
    {
        if (sources.TryGetValue(type, out var source) && source != null)
        {
            return source.volume;
        }
        return 0f;
    }

    public void DecreaseVolume(SoundType type, float amount)
    {
        SetVolume(type, GetVolume(type) - amount);
    }
}