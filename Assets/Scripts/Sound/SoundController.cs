using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

enum SoundSequence
{
    MainSceneButtonClick01,
    MainSceneButtonClick02,
    MainSceneButtonLick01,
    MainSceneButtonLick02,
    MainSceneButtonLick03,
    StageSelectSceneButtonClick01,
    StageSelectSceneButtonClick02,
    StageSelectSceneButtonClick03,
    StageSelectSceneButtonLick01,
    StageSelectSceneButtonLick02,
    StageSelectSceneButtonLick03,
    StageSelectSceneButtonLick04,
    StageSelectSceneButtonLick05,
    TitleSceneButtonClick01,
    UnitContainerSceneButtonClick01,
    UnitContainerSceneButtonClick02
}

public class SoundController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector]
    public AudioClip[] uiClip;

    [Space(10)]

    [Header("TitleScene")]
    public TitleSceneSound titleScene;

    [Serializable]
    public class TitleSceneSound
    {
        public bool[] buttonClick, buttonLick;
    }

    [Space(10)]

    [Header("MainScene")]
    public MainSceneSound mainScene;

    [Serializable]
    public class MainSceneSound
    {
        public bool[] buttonClick, buttonLick;
    }

    [Space(10)]

    [Header("StageSelectScene")]
    public StageSelectSceneSound stageSelectScene;

    [Serializable]
    public class StageSelectSceneSound
    {
        public bool[] buttonClick, buttonLick;
    }

    [Space(10)]

    [Header("UnitContainerScene")]
    public UnitContainerSceneSound unitContainerScene;

    [Serializable]
    public class UnitContainerSceneSound
    {
        public bool[] buttonClick, buttonLick;
    }

    [Space(10)]

    [Header("DefenceStageScene")]
    public DefenceStageSceneSound defenceStageScene;

    [Serializable]
    public class DefenceStageSceneSound
    {
        public bool[] buttonClick, buttonLick;
    }

    private void Start()
    {
        uiClip = Resources.LoadAll<AudioClip>("Sounds/UI sound/");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {

        }

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (mainScene.buttonLick[0])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.MainSceneButtonLick01]);

            if (mainScene.buttonLick[1])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.MainSceneButtonLick02]);

            if (mainScene.buttonLick[2])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.MainSceneButtonLick03]);
        }

        if (SceneManager.GetActiveScene().name == "StageSelectScene")
        {
            if (stageSelectScene.buttonLick[0])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick01]);

            if (stageSelectScene.buttonLick[1])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick02]);

            if (stageSelectScene.buttonLick[2])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick03]);

            if (stageSelectScene.buttonLick[3])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick04]);

            if (stageSelectScene.buttonLick[4])
                SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonLick05]);
        }

        if (SceneManager.GetActiveScene().name == "UnitContainerScene")
        {
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (titleScene.buttonClick[0])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.TitleSceneButtonClick01]);
            }
        }

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (mainScene.buttonClick[0])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.MainSceneButtonClick01]);

                if (mainScene.buttonClick[1])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.MainSceneButtonClick02]);
            }
        }

        if (SceneManager.GetActiveScene().name == "StageSelectScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (stageSelectScene.buttonClick[0])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonClick01]);

                if (stageSelectScene.buttonClick[1])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonClick02]);

                if (stageSelectScene.buttonClick[2])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.StageSelectSceneButtonClick03]);
            }
        }

        if (SceneManager.GetActiveScene().name == "UnitContainerScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (unitContainerScene.buttonClick[0])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.UnitContainerSceneButtonClick01]);

                if (unitContainerScene.buttonClick[1])
                    SoundManager.Instance.PlayEffectsSound(uiClip[(int)SoundSequence.UnitContainerSceneButtonClick02]);
            }
        }

        if (SceneManager.GetActiveScene().name == "DefenceStageScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {

            }
        }
    }
}
