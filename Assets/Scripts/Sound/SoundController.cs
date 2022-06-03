using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

enum SoundSequence
{
    MainSceneButtonClick,
    MainSceneButtonLick,
    StageSelectSceneButtonClick01,
    StageSelectSceneButtonClick02,
    StageSelectSceneButtonClick03,
    StageSelectSceneButtonLick01,
    StageSelectSceneButtonLick02,
    StageSelectSceneButtonLick03,
}

public class SoundController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector]
    public AudioClip[] uiClip;

    [Header("MainScene")]
    public MainSceneSound mainScene;

    [Serializable]
    public class MainSceneSound
    {
        public bool buttonLick, buttonClick;
    }

    [Space(10)]

    [Header("StageSelectScene")]
    public StageSelectSceneSound stageSelectScene;

    [Serializable]
    public class StageSelectSceneSound
    {
        public bool buttonClick01, buttonClick02, buttonClick03, buttonLick01, buttonLick02, buttonLick03;
    }

    private void Start()
    {
        uiClip = Resources.LoadAll<AudioClip>("Sounds/UI sound/");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (mainScene.buttonLick)
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.MainSceneButtonLick]);
        }

        if (SceneManager.GetActiveScene().name == "StageSelectScene")
        {
            if (stageSelectScene.buttonLick01)
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick01]);

            if (stageSelectScene.buttonLick02)
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick02]);

            if (stageSelectScene.buttonLick03)
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick03]);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (mainScene.buttonClick)
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.MainSceneButtonClick]);
            }
        }

        if (SceneManager.GetActiveScene().name == "StageSelectScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (stageSelectScene.buttonClick01)
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonClick01]);

                if (stageSelectScene.buttonClick02)
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonClick02]);

                if (stageSelectScene.buttonClick03)
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonClick03]);
            }
        }
    }
}
