using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using Diction;


[System.Serializable]
public class ImageFile
{
    //public MinionsInfo info;
    public Sprite Illusts;
    public Sprite LvFrame;
    public Sprite RankFrame;
    public Sprite RankStar;
    public Sprite ClassSimbol;

    public ImageFile()
    {
        Illusts = null;
        LvFrame = null;
        RankFrame = null;
        RankStar = null;
        ClassSimbol = null;
    }
}

[System.Serializable]
public struct MinionsInfo
{
    public int Num;
    public string k_name;
    public string e_name;
    public int Lv;
    public string Class;
    public string Camp;
    public RANK rank;

    public MinionsInfo(int _num = -1 , string k_name = "", string e_name = "",
        int _lv = -1, string _Class ="", string _Camp ="", int _rank = -1)
    {
        Num = _num;
        this.k_name = k_name;
        this.e_name = e_name;
        this.Lv = _lv;
        this.Class = _Class;
        this.Camp = _Camp;
        this.rank = (RANK)_rank;
    }
    public int pro_num
    {
        get
        {
            return Num;
        }
        set
        {
            Num = value;
        }
    }
    public string pro_k_name
    {
        get
        {
            return k_name;
        }
        set
        {
            k_name = value;
        }
    }
    public string pro_e_name
    {
        get
        {
            return e_name;    
        }
        set
        {
            e_name = value;
        }
    }
    public int pro_lv
    {
        get
        {
            return Lv;
        }
        set
        {
            Lv = value;
        }
    }
    public string pro_class
    {
        get { return Class; }
        set { Class = value; }
    }
    public string pro_Camp
    {
        get { return Camp; }
        set { Camp = value; }
    }
    public RANK pro_rank
    {
        get { return rank; }
        set { rank = value; }
    }
}



public class MinionsPortrait : MonoBehaviour
{
    [System.Serializable]
    public class SerializeDic : PublicDictionary.MinionsData<string, ImageFile> { }
    [Header("스탠딩 일러스트 좌표")]
    public MinionsPortrait pppp;

    [Header("BackGround")] 
    [SerializeField]
    public Sprite NoneBG;           // portrait에 아무것도 존재하지 않을 경우 NoneBG로 출력
    [SerializeField]
    public Sprite PortraitBg;       // portrait에 무엇인가 존재하면 PortraitBG로 출력

    [Space(10f)]
    [Header("Text UI")]
    [SerializeField]
    private TextMeshProUGUI CampText;
    [SerializeField]
    private TextMeshProUGUI ClassText;
    [SerializeField]
    private TextMeshProUGUI Name_Text;
    [SerializeField]
    private TextMeshProUGUI Lv_Text;

    [Space(10f)]
    [Header("Illust UI")]
    [SerializeField]
    private Image ShowIllust;

    [Space(10f)]
    [Header("Rank UI")]
    [SerializeField]
    private Image ClassFrame;
    [SerializeField]
    private Image LevelFrame;
    [SerializeField]
    private Image RankFrame;
    [SerializeField]
    private Image StarFrame;

    [Space(20f)]
    [Header("Data")]
    [SerializeField]
    public SerializeDic miss = new SerializeDic();
    [SerializeField]
    public MinionsInfo m_info;
    

    private void Start()
    {
        //if (info_Panel != null)
        //    info_Panel.SetActive(false);
        init();
    }

    void initText()
    {
        if (CampText != null)
        {
            CampText.gameObject.SetActive(true);
            CampText.text = m_info.Camp;
        }
        if (ClassText != null)
        {
            ClassText.gameObject.SetActive(true);
            ClassText.text = m_info.Class;
        }
        if (Name_Text != null)
        {
            Name_Text.gameObject.SetActive(true);
            Name_Text.text = m_info.k_name;
        }
        if(Lv_Text != null)
        {
            Lv_Text.gameObject.SetActive(true);
            if(this.gameObject.tag == "Minions_Inventory")
               Lv_Text.text = "LV: " + m_info.Lv.ToString() ;
            else
                Lv_Text.text = m_info.Lv.ToString();
        }
    }

    void initUI()         // 랭크 및 클래스와 관련된 UI 업데이트
    {
        Sprite sp;
        if (ClassFrame != null)
        {
            ClassFrame.gameObject.SetActive(true);
            sp = miss[m_info.pro_e_name].ClassSimbol;
            if (sp != null)
                ClassFrame.sprite = sp;
            else
                throw new System.Exception("Class Simbol Error");
        }
        if(LevelFrame != null)
        {
            LevelFrame.gameObject.SetActive(true);
            sp = miss[m_info.pro_e_name].LvFrame;
            switch (this.gameObject.tag)
            {
                case "Minions_Inventory":
                    if (sp != null)
                        this.gameObject.GetComponent<Image>().sprite = sp;
                    break;
                case "Minions_EditList":
                case "Minions_Standing":
                    if (sp != null)
                        LevelFrame.sprite = sp;
                    else
                        throw new System.Exception("LevelFrame Error");
                    break;
                default:
                    break;
            }
        }
        if(StarFrame != null)
        {
            StarFrame.gameObject.SetActive(true);
            //StarFrame.sprite = miss[m_info.pro_e_name].RankStar;
            sp = miss[m_info.pro_e_name].RankStar;
            if (sp != null)
                StarFrame.sprite = sp;
            else
                throw new System.Exception("StarFrame Error");
        }
        if(RankFrame != null)
        {
            RankFrame.gameObject.SetActive(true);
            sp = miss[m_info.pro_e_name].RankFrame;
            if (sp != null)
                RankFrame.sprite = sp;
            else
                throw new System.Exception("RankFrame Error");
        }
    }
    
    void hideinit()
    {
        if (CampText != null)
        {
            CampText.gameObject.SetActive(false);
        }
        if (ClassText != null)
        {
            ClassText.gameObject.SetActive(false);
        }
        if (Name_Text != null)
        {
            Name_Text.gameObject.SetActive(false);
        }
        if (Lv_Text != null)
        {
            Lv_Text.gameObject.SetActive(false);
        }
        if(ClassFrame != null)
        {
            ClassFrame.gameObject.SetActive(false);
        }
        if (StarFrame != null)
        {
            StarFrame.gameObject.SetActive(false);
        }
        if(LevelFrame != null)
        {
            LevelFrame.gameObject.SetActive(false);
        }
        if(RankFrame != null)
        {
            RankFrame.gameObject.SetActive(false);
        }
    }

    public void init()
    {
        try
        {
            // 현재 Portrait에 유닛의 정보가 존재하는가? 존재하지 않으면 -1를 반환
            if (m_info.Num == -1)
            {
                this.gameObject.GetComponent<Image>().sprite = NoneBG;
                if(ShowIllust != null   )
                {
                    ShowIllust.gameObject.SetActive(false); 
                }
                hideinit();
            }
            else
            {
                this.gameObject.GetComponent<Image>().sprite = PortraitBg;      // 배경 변경

                initText();
                if(ShowIllust != null)
                {
                    ShowIllust.gameObject.SetActive(true);
                    ShowIllust.sprite = miss[m_info.pro_e_name].Illusts ;       // Key번째 일러스트가 출력
                }
                initUI();
            }
            if (this.gameObject.GetComponent<Button>() != null)
            {
                if (this.tag == "Minions_Inventory")
                {
                    pppp = GameObject.Find("SelectedStanding").GetComponent<MinionsPortrait>();
                    this.gameObject.GetComponent<Button>().onClick.AddListener(() => pppp.ShowStanding(this.m_info));      // 스탠딩에 정보를 보여주는 기능
                                                                                                                           //LobbyUIManager.Instance.

                }
                else if (this.tag == "Minions_EditList")
                    Debug.Log("EditList 기능 미구현");                                                      // 에딧 리스트의 portrait을 누를 시 적용되는 기능 
                else if (this.tag == "Minions_Standing")
                {
                    Debug.Log("Standing 기능 미구현");                                             // 스탠딩을 누를 시 정보를 보여주는 기능
                    //this.gameObject.GetComponent<Button>().onClick.AddListener(() => ShowInfo());
                    //this.gameObject.GetComponent<Button>().onClick.AddListener(
                    //    () => LobbyUIManager.Instance.ShowPanel());
                }
            }
            else
            {
                Debug.Log("버튼 없는 편");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void addDictionary(string Key)
    {
        Debug.Log("Add Dictionary");
        ImageFile im = new ImageFile();
        miss.Add(Key, im);       // 지속적으로 추가해주는 스크립트
    }

    public void RemoveDictionary(string key)
    {
        if(miss.ContainsKey(key))
        {
            miss.Remove(key);
        }
    }

    // properties
    public int pro_num
    {
        get { return m_info.pro_num; }
    }

    public string pro_k_name
    {
        get { return m_info.pro_k_name; }
        //set { m_info.pro_k_name = value; }
    }

    public string pro_e_name
    {
        get { return m_info.pro_e_name; }
    }

    public int pro_lv
    {
        get { return m_info.pro_lv; }
    }

    public string pro_Class
    {
        get { return m_info.pro_class; }
    }

    public string pro_Camp
    {
        get { return m_info.pro_Camp; }
    }

    public RANK pro_rank
    {
        get { return m_info.pro_rank; }
    }

    public MinionsInfo pro_info
    {
        get { return m_info; }
        set { m_info = value; }
    }

    public MinionsInfo PortraitCopy()
    {
        return new MinionsInfo(this.m_info.pro_num, this.m_info.pro_k_name, 
            this.m_info.pro_e_name, this.m_info.pro_lv, this.m_info.pro_class,
            this.m_info.pro_Camp, (int)this.m_info.pro_rank);
    }

    internal void updateInfo(MinionsInfo info)
    {
        m_info = info;
    }

    public void ShowStanding(MinionsInfo _info)
    {
        m_info = _info;
        init();
    }

    public void ShowInfo()
    {
        Debug.Log("유닛의 정보 보여주기");
    }


}
