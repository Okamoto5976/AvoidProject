using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //ReferenceObject
    [Header("Necessary Reference")]
    [SerializeField] private GameObject m_menu;
    [SerializeField] private GameObject m_option;

    //VolumeControlSlider
    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;

    private string m_mixerMasterName = "MasterVolume";
    private string m_mixerBGMName = "BGMVolume";
    private string m_mixerSEName = "SEVolume";

    [SerializeField] private AudioMixer m_mixer;
    //InputController

    [Header("Not Necessary Reference")]
    private GameManager m_gameManager;

    private OptionSaveClass m_optionSaveClass;

    //[SerializeField] private 
    //GoreController

    //InputProvider

    public void Initialized()
    {
        //OptionSaveÇ©ÇÁåªç›ê›íËÇ≥ÇÍÇƒÇ¢ÇÈVolumeÇMixerÇ∆SliderÇ…ê›íË

        m_optionSaveClass = new();

        OptionSaveData data = m_optionSaveClass.LoadOptionData();

        if(data != null)
        {
            m_mixer.SetFloat(m_mixerMasterName, data.m_masterVolume);
            m_mixer.SetFloat(m_mixerBGMName, data.m_bgmVolume);
            m_mixer.SetFloat(m_mixerSEName, data.m_seVolume);
        }
        else
        {
            m_mixer.SetFloat(m_mixerMasterName, 0f);
            m_mixer.SetFloat(m_mixerBGMName, 0f);
            m_mixer.SetFloat(m_mixerSEName, 0f);
        }

        SetValueOfSlider(m_mixerMasterName, m_masterSlider);
        SetValueOfSlider(m_mixerBGMName, m_bgmSlider);
        SetValueOfSlider(m_mixerSEName, m_seSlider);

    }

    private void SetValueOfSlider(string name, Slider slider)
    {
        float db;

        m_mixer.GetFloat(name, out db);

        float volume = Mathf.Pow(10f, db / 20f);

        slider.value = volume;
    }

    public void OnSetMasterVolumeControl(float value)
    {
        if(value <= 0f)
        {
            m_mixer.SetFloat(m_mixerMasterName, -80f);
        }
        else
        {
            m_mixer.SetFloat(m_mixerMasterName, Mathf.Log10(value) * 20);
        }

        SaveOption();
    }

    public void OnSetBGMVolumeControl(float value)
    {
        if (value <= 0f)
        {
            m_mixer.SetFloat(m_mixerBGMName, -80f);
        }
        else
        {
            m_mixer.SetFloat(m_mixerBGMName, Mathf.Log10(value) * 20);
        }

        SaveOption();

    }

    public void OnSetSEVolumeControl(float value)
    {
        if (value <= 0f)
        {
            m_mixer.SetFloat(m_mixerSEName, -80f);
        }
        else
        {
            m_mixer.SetFloat(m_mixerSEName, Mathf.Log10(value) * 20);
        }

        SaveOption();
    }

    private void SaveOption()
    {
        OptionSaveData data = new();

        m_mixer.GetFloat(m_mixerMasterName, out float masterValue);
        data.m_masterVolume = masterValue;
        m_mixer.GetFloat(m_mixerBGMName, out float bgmValue);
        data.m_bgmVolume = bgmValue;
        m_mixer.GetFloat(m_mixerSEName, out float seValue);
        data.m_seVolume = seValue;

        m_optionSaveClass.SaveOptionData(data);
    }

    public void OnOpenMenu()
    {

        //GameManager State Pouse
    }

    public void OnCloseMenu()
    {
        //GameManager State InPlay
    }

    public void OnOpenOption()
    {

    }

    public void OnCloseOption()
    {

    }

    public void SetGameManager(GameManager component)
    {
        m_gameManager = component;
    }
}
