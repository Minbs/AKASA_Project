using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public GameObject soundCanvas;
    public AudioClip[] bgmClipList;

    [SerializeField]
    private AudioSource bgmSource, effectsSource;
    [SerializeField]
    private Slider masterSlider, bgmSlider, effectsSlider;
    [SerializeField]
    private GameObject masterCheck, bgmCheck, effectsCheck;
    private bool canvasCheck;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (soundCanvas.activeSelf) soundCanvas.SetActive(false);

        ChangeMasterVolume(masterSlider.value);
        ChangeBGMVolume(bgmSlider.value);
        ChangeEffectsVolume(effectsSlider.value);

        masterSlider.onValueChanged.AddListener(val => ChangeMasterVolume(val));
        bgmSlider.onValueChanged.AddListener(val => ChangeBGMVolume(val));
        effectsSlider.onValueChanged.AddListener(val => ChangeEffectsVolume(val));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnKeyCheck();
            soundCanvas.SetActive(canvasCheck);
        }
    }

    private void OnKeyCheck() => canvasCheck = soundCanvas.activeSelf == false ? true : false;

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgmClipList.Length; i++)
            if (arg0.name + "_BGM_01" != bgmClipList[i].name)
                PlayBGMSound(bgmClipList[1]);

        for (int i = 0; i < bgmClipList.Length; i++)
            if (arg0.name + "_BGM_01" == bgmClipList[i].name)
                PlayBGMSound(bgmClipList[i]);
    }

    public void PlayBGMSound(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayEffectsSound(AudioClip clip)
    {
        effectsSource.clip = clip;
        effectsSource.loop = false;
        effectsSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ChangeBGMVolume(float value)
    {
        bgmSource.volume = value / 10000;
    }

    public void ChangeEffectsVolume(float value)
    {
        effectsSource.volume = value / 10000;
    }

    public void ToggleMuteMaster()
    {
        if (masterCheck.GetComponent<Toggle>().isOn)
        {
            bgmSource.mute = false;
            effectsSource.mute = false;

            if (!bgmCheck.GetComponent<Toggle>().isOn)
                bgmSource.mute = true;
            if (!effectsCheck.GetComponent<Toggle>().isOn)
                effectsSource.mute = true;
        }
        else
        {
            bgmSource.mute = true;
            effectsSource.mute = true;
        }
    }

    public void ToggleMuteBGM()
    {
        if (masterCheck.GetComponent<Toggle>().isOn)
            bgmSource.mute = !bgmSource.mute;
    }

    public void ToggleMuteEffects()
    {
        if (masterCheck.GetComponent<Toggle>().isOn)
            effectsSource.mute = !effectsSource.mute;
    }
}
