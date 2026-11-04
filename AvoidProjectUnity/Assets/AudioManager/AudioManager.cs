using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音の種類。Stopで止める対象を指定するときに使う。
/// </summary>
public enum SoundType
{
    BGM,
    SE
}

/// <summary>
/// 渡されたAudioDataを再生・停止するクラス。
/// 音量の調整はここでは行わない（別の場所で制御する）。
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>BGM再生用のAudioSource</summary>
    [SerializeField] private AudioSource m_bgmSource;

    /// <summary>SE再生用のAudioSource。プールの1つ目として使い、足りなければ同じ設定で追加する</summary>
    [SerializeField] private AudioSource m_seSource;

    /// <summary>SEを同時に鳴らせる最大数（AudioSourceの上限）</summary>
    [SerializeField] private int m_maxSESources = 16;

    /// <summary>実行中のBGMフェード処理。止めるときにキャンセルするため保持する</summary>
    private Coroutine m_bgmFadeCoroutine;

    /// <summary>SE用AudioSourceのプール。空いているものを使い回す</summary>
    private readonly List<AudioSource> m_sePool = new List<AudioSource>();

    private void Awake()
    {
        if (m_seSource != null)
        {
            m_sePool.Add(m_seSource);
        }
    }

    /// <summary>
    /// BGMを再生する。再生中なら今のBGMをフェードアウトしてから、次のBGMをフェードインする。
    /// 同じBGMがすでに再生中なら何もしない。
    /// </summary>
    /// <param name="data">再生するBGMのAudioData</param>
    /// <param name="fadeTime">フェードアウト・フェードインそれぞれにかける秒数</param>
    public void PlayBGM(AudioData data, float fadeTime = 1f)
    {
        if (m_bgmSource == null || data == null || data.Clip == null) return;

        //同じBGMが再生中なら何もしない
        if (m_bgmSource.clip == data.Clip && m_bgmSource.isPlaying) return;

        CancelBGMFade();
        m_bgmFadeCoroutine = StartCoroutine(PlayBGMRoutine(data, fadeTime));
    }

    /// <summary>
    /// BGMをフェードアウトして止める。
    /// </summary>
    /// <param name="fadeTime">フェードアウトにかける秒数</param>
    public void FadeOutBGM(float fadeTime = 1f)
    {
        if (m_bgmSource == null) return;

        CancelBGMFade();
        m_bgmFadeCoroutine = StartCoroutine(FadeOutBGMRoutine(fadeTime));
    }

    /// <summary>
    /// SEを再生する。空いているAudioSourceで鳴らすので、複数のSEが重なって再生できる。
    /// 空きが無く上限（m_maxSESources）に達している場合は鳴らさない。
    /// </summary>
    /// <param name="data">再生するSEのAudioData</param>
    public void PlaySE(AudioData data)
    {
        if (data == null || data.Clip == null) return;

        AudioSource source = GetFreeSESource();
        if (source == null) return;

        Apply(source, data);
        source.Play();
    }

    /// <summary>
    /// 指定した種類の音を止める。BGMのフェード中なら、フェードも中止する。
    /// </summary>
    /// <param name="type">止める対象（BGM or SE）</param>
    public void Stop(SoundType type)
    {
        switch (type)
        {
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

    /// <summary>
    /// BGMとSEを全て止める。
    /// </summary>
    public void StopAll()
    {
        Stop(SoundType.BGM);
        Stop(SoundType.SE);
    }

    /// <summary>
    /// AudioDataの設定（clip / volume / pitch / loop）をAudioSourceに反映する。
    /// </summary>
    private static void Apply(AudioSource source, AudioData data)
    {
        source.clip = data.Clip;
        source.volume = data.Volume;
        source.pitch = data.Pitch;
        source.loop = data.Loop;
    }

    /// <summary>
    /// AudioSourceを止めて、volume / pitch / loopを初期値に戻す。
    /// </summary>
    private static void StopSource(AudioSource source)
    {
        if (source == null) return;

        source.Stop();
        source.volume = 1f;
        source.pitch = 1f;
        source.loop = false;
    }

    /// <summary>
    /// 再生中でないSE用AudioSourceを返す。無ければ上限まで追加し、上限に達していればnullを返す。
    /// 追加したAudioSourceは、m_seSourceと同じMixer Groupに出力する。
    /// </summary>
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

    /// <summary>
    /// 実行中のBGMフェード処理があれば止める。
    /// </summary>
    private void CancelBGMFade()
    {
        if (m_bgmFadeCoroutine == null) return;

        StopCoroutine(m_bgmFadeCoroutine);
        m_bgmFadeCoroutine = null;
    }

    /// <summary>
    /// 今のBGMをフェードアウトし、次のBGMをフェードインで再生する。
    /// フェードインの到達音量は、AudioDataのvolumeになる。
    /// </summary>
    private IEnumerator PlayBGMRoutine(AudioData data, float fadeTime)
    {
        if (m_bgmSource.isPlaying)
        {
            yield return FadeBGM(m_bgmSource.volume, 0f, fadeTime);
        }
        m_bgmSource.Stop();

        Apply(m_bgmSource, data);
        m_bgmSource.volume = 0f;
        m_bgmSource.Play();
        yield return FadeBGM(0f, data.Volume, fadeTime);

        m_bgmFadeCoroutine = null;
    }

    /// <summary>
    /// 今のBGMをフェードアウトして止める。
    /// </summary>
    private IEnumerator FadeOutBGMRoutine(float fadeTime)
    {
        if (m_bgmSource.isPlaying)
        {
            yield return FadeBGM(m_bgmSource.volume, 0f, fadeTime);
        }
        StopSource(m_bgmSource);

        m_bgmFadeCoroutine = null;
    }

    /// <summary>
    /// BGMのAudioSourceのvolumeを、fromからtoまで時間をかけて変化させる。
    /// ポーズ中（timeScale=0）でも進むようにunscaledDeltaTimeを使う。
    /// </summary>
    /// <param name="duration">かける秒数（0以下なら即座にtoにする）</param>
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
}