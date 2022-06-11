using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Diction;


[System.Serializable]
public struct Privacy   
{
    public string age;      // 불명도 존재하니 string으로 설정
    public string sex;      // 성별
    public string height;   // 키
    public string weight;    //
    public string tribe;    // 종족
}

[System.Serializable]
public class FlavorText
{
    public string Codename;
    public string k_name;
    public string e_name;
    public string Coment;
    public Privacy privacy;
    public string className;
    public string Story;
    public string skillname;

    public Sprite ClassSimbol;
    public Sprite SkillSImbol;
}


public class InfoPopup : Singleton<InfoPopup>
{
    // 본 스크립트는 미니언의 정보(능력치 X)가 기입되어있는
    // 패널을 관리하는 스크립트데 아루;

    [System.Serializable]
    public class SerializeDic : PublicDictionary.MinionsData<string, FlavorText> { }
    public MinionsPortrait InfoTarget;


    [Header("TextMeshPro")]
    [SerializeField]
    public TextMeshProUGUI CodeNameText;
    public TextMeshProUGUI Minion_KNameText;
    public TextMeshProUGUI Minion_ENameText;
    public TextMeshProUGUI CharInfoText;
    public TextMeshProUGUI ClassNameText;
    public TextMeshProUGUI ComentText;
    //public TextMeshProUGUI StoryTitleText;
    public TextMeshProUGUI StoryText;
    public TextMeshProUGUI skillName;


	public int lineSpace = 30;

    [Space(10f)]
    [Header("Image Sprite")]
    [SerializeField]
    //public List<Sprite> ClassList;
    public Image ClassSimbol;
    public Image SkillSimbol;

    [Space(20f)]
    [Header("data")]
    [SerializeField]
    public SerializeDic Infos = new SerializeDic();
    //public int a;
    private void Start()
    {
        //InitData();
    }

    private void OnEnable()
    {
        InitData();
    }


    public void InitText(string key)
    {
        if(CodeNameText != null)
            CodeNameText.text = Infos[key].Codename;
        if (Minion_KNameText != null)
            Minion_KNameText.text = Infos[key].k_name;
        if (Minion_ENameText != null)
            Minion_ENameText.text = Infos[key].e_name;
        if (CharInfoText != null)
            CharInfoText.text = Infos[key].privacy.age + " / " + Infos[key].privacy.sex + " / " +
                Infos[key].privacy.height + " / " + Infos[key].privacy.weight + " / " + Infos[key].privacy.tribe;
        if (ClassNameText != null)
            ClassNameText.text = Infos[key].className;
        if (ComentText != null)
            ComentText.text = Infos[key].Coment;
        if (StoryText != null)
            StoryText.text = Infos[key].Story.Replace("\\n", "\n");
        if (skillName != null)
            skillName.text = Infos[key].skillname;

	StoryText.lineSpacing = lineSpace;
    }

    public void InitSimbol(string key)
    {
        if (ClassSimbol != null)
            ClassSimbol.sprite = Infos[key].ClassSimbol;
        if (SkillSimbol != null)
            SkillSimbol.sprite = Infos[key].SkillSImbol;
    } 
    public void InitData()
    {
        string key = InfoTarget.pro_e_name;
        try
        {
            if(InfoTarget.pro_num == -1)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                InitText(key);
                InitSimbol(key);
                //Debug.Log("Hello World!");
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }




    public void addDictionary(string Key)
    {
        Debug.Log("Add Dictionary");
        FlavorText im = new FlavorText();
        Infos.Add(Key, im);       // 지속적으로 추가해주는 스크립트
    }

    public void RemoveDictionary(string key)
    {
        if (Infos.ContainsKey(key))
        {
            Infos.Remove(key);
        }
    }


}
