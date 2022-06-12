using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

enum SoundSequence
{
    DefenceStageSceneButtonClick01,
    DefenceStageSceneButtonClick02,
    DefenceStageSceneButtonClick03,
    MainSceneButtonClick01,
    MainSceneButtonClick02,
    MainSceneButtonClick03,
    MainSceneButtonLick01,
    MainSceneButtonLick02,
    MainSceneButtonLick03,
    StageSelectSceneButtonClick01,
    StageSelectSceneButtonClick02,
    StageSelectSceneButtonClick03,
    StageSelectSceneButtonClick04,
    StageSelectSceneButtonClick05,
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
    //[HideInInspector]
    public AudioClip[] uiClip;
    public string[] uiClipName;
    public AudioClip[] skillClip;
    public string[] skillClipName;

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

    [Header("Skill")]
    public SkillSound skill;

    [Serializable]
    public class SkillSound
    {
        public bool[] sound;
    }

    private void Start()
    {
        uiClip = Resources.LoadAll<AudioClip>("Sounds/UI sound/");
        uiClipName = new string[uiClip.Length];
        skillClip = Resources.LoadAll<AudioClip>("Sounds/Skill sound/");
        skillClipName = new string[skillClip.Length];

        for (int i = 0; i < uiClip.Length; i++)
        {
            uiClipName[i] = uiClip[i].name.ToString();
        }
        for (int i = 0; i < skillClip.Length; i++)
        {
            skillClipName[i] = skillClip[i].name.ToString();
        }
    }

    public void PlayUISound(string name)
    {
        int index = 0;

        for (int i = 0; i < uiClip.Length; i++)
        {
            if (name == uiClipName[i].ToString())
            {
                index = i;
            }
        }

        SoundManager.Instance.PlayEffectsSound(uiClip[index]);
    }

    public void PlaySkillSound(string name)
    {
        int index = 0;

        for (int i = 0; i < skillClip.Length; i++)
        {
            if (name == skillClipName[i].ToString())
            {
                index = i;
            }
        }

        SoundManager.Instance.PlayEffectsSound(skillClip[index]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {

        }

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (mainScene.buttonLick[0])
                PlayUISound("MainScene_Button_Lick_01");

            if (mainScene.buttonLick[1])
                PlayUISound("MainScene_Button_Lick_02");

            if (mainScene.buttonLick[2])
                PlayUISound("MainScene_Button_Lick_03");
        }

        if (SceneManager.GetActiveScene().name == "StageSelectScene_2")
        {
            if (stageSelectScene.buttonLick[0])
                PlayUISound("StageSelectScene_Button_Lick_01");

            if (stageSelectScene.buttonLick[1])
                PlayUISound("StageSelectScene_Button_Lick_02");

            if (stageSelectScene.buttonLick[2])
                PlayUISound("StageSelectScene_Button_Lick_03");

            if (stageSelectScene.buttonLick[3])
                PlayUISound("StageSelectScene_Button_Lick_04");

            if (stageSelectScene.buttonLick[4])
                PlayUISound("StageSelectScene_Button_Lick_05");
        }

        if (SceneManager.GetActiveScene().name == "UnitContainerScene")
        {
            if (unitContainerScene.buttonLick[0])
                PlayUISound("UnitContainerScene_Button_Lick_01");

            if (unitContainerScene.buttonLick[1])
                PlayUISound("UnitContainerScene_Button_Lick_02");

            if (unitContainerScene.buttonLick[2])
                PlayUISound("UnitContainerScene_Button_Lick_03");

            if (unitContainerScene.buttonLick[3])
                PlayUISound("UnitContainerScene_Button_Lick_04");

            if (unitContainerScene.buttonLick[4])
                PlayUISound("UnitContainerScene_Button_Lick_05");

            if (unitContainerScene.buttonLick[5])
                PlayUISound("UnitContainerScene_Button_Lick_06");
        }

        if (SceneManager.GetActiveScene().name == "DefenceStageScene")
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
                    PlayUISound("TitleScene_Button_Click_01");
            }
        }

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (mainScene.buttonClick[0])
                    PlayUISound("MainScene_Button_Click_01");

                if (mainScene.buttonClick[1])
                    PlayUISound("MainScene_Button_Click_02");

                if (mainScene.buttonClick[2])
                    PlayUISound("MainScene_Button_Click_03");
            }
        }

        if (SceneManager.GetActiveScene().name == "StageSelectScene_2")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (stageSelectScene.buttonClick[0])
                    PlayUISound("StageSelectScene_Button_Click_01");

                if (stageSelectScene.buttonClick[1])
                    PlayUISound("StageSelectScene_Button_Click_02");

                if (stageSelectScene.buttonClick[2])
                    PlayUISound("StageSelectScene_Button_Click_03");

                if (stageSelectScene.buttonClick[3])
                    PlayUISound("StageSelectScene_Button_Click_04");

                if (stageSelectScene.buttonClick[4])
                    PlayUISound("StageSelectScene_Button_Click_05");
            }
        }

        if (SceneManager.GetActiveScene().name == "UnitContainerScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (unitContainerScene.buttonClick[0])
                    PlayUISound("UnitContainerScene_Button_Click_01");

                if (unitContainerScene.buttonClick[1])
                    PlayUISound("UnitContainerScene_Button_Click_02");

                if (unitContainerScene.buttonClick[2])
                    PlayUISound("UnitContainerScene_Button_Click_03");

                if (unitContainerScene.buttonClick[3])
                    PlayUISound("UnitContainerScene_Button_Click_04");

                if (unitContainerScene.buttonClick[4])
                    PlayUISound("UnitContainerScene_Button_Click_05");

                if (unitContainerScene.buttonClick[5])
                    PlayUISound("UnitContainerScene_Button_Click_06");
            }
        }

        if (SceneManager.GetActiveScene().name == "DefenceStageScene")
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (defenceStageScene.buttonClick[0])
                    PlayUISound("DefenceStageScene_Button_Click_01");

                if (defenceStageScene.buttonClick[1])
                    PlayUISound("DefenceStageScene_Button_Click_02");

                if (defenceStageScene.buttonClick[2])
                    PlayUISound("DefenceStageScene_Button_Click_03");
            }
        }
    }
}
