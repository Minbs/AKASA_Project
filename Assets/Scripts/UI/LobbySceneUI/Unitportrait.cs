using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum RANK
{
    RANK_RARE =0,
    RANK_EPIC = 1,
    RANK_LEGEND = 2
}
public enum MINION_CLASS
{
    CLASS_GUARDIAN = 0 ,
    CLASS_RESCUE = 1,
    CLASS_MAGE = 2,
    CLASS_BUSTER = 3,
    CLASS_CHASER = 4 ,
    CLASS_PALADIN = 5,
}

public class Unitportrait : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private Sprite Minions_List_BG;        // 유닛 리스트 뒷배경
    [SerializeField] private Sprite EditBGSprite;           // 편집창 뒷배경
    [SerializeField] private Sprite ShowStanding;           // 스탠딩 뒷배경
    [SerializeField] private TextMeshProUGUI CampText;
    [SerializeField] private TextMeshProUGUI ClassText;
    [SerializeField] private TextMeshProUGUI MinionName;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Image ClassFrame;
    [SerializeField] private Image LevelFrame;
    [SerializeField] private Image StarImage;

    [Header("Rank Sprite")]
    [SerializeField] private List<Sprite> Level_Standing;
    [SerializeField] private List<Sprite> Level_Sprite;
    [SerializeField] private List<Sprite> Level_Inventory;
    [SerializeField] private List<Sprite> RankStar;

    [Header("Unit Class")]
    [SerializeField] private List<Sprite> ClassSprite;

    [Space(10f)]
    [Header ("넘버")]
    [SerializeField] private int Minion_num;
    [Space(10f)]
    [Header("이름")]
    [SerializeField]  private string Minion_k_Name;
    [SerializeField] private string Minion_e_Name;
    [Space(10f)]
    [Header("진영")]
    [SerializeField] private string e_camp;
    [Space (10f)]
    [Header("클래스")]
    [SerializeField] private string Minion_ClassName;
    [SerializeField] private MINION_CLASS MinionClass;
    [Space (10f)]
    [Header("레벨")]
    [SerializeField]  private int Minioun_Lv;
    [Space(10f)]
    [Header("등급")]
    [SerializeField] private RANK MInion_Rank; // enum이 존재할 경우 enum으로 변경
    [Space(10f)]
    [Header("코스트")]
    [SerializeField] private int Cost;
    [Space(10f)]
    [Header("타이머")]
    [SerializeField] private float ReturnTimer;     // 캐릭터 반환시 받는 타이머
    [SerializeField] private float RelocationTimer;

    [Space(10f)]
    [Header("스테이터스")]
    [SerializeField] private float Hp;
    [SerializeField] private float AtSpeed;
    [SerializeField] private float Atk;
    [SerializeField] private float Def;
    [SerializeField] private float Critical;

    public int getCount; // 획득 날짜.


    // 임시로 값을 설정
    private void Start()
    {
        
        //UnitSprite = this.gameObject.GetComponent<Image>().sprite;

    }

    public void UpdateUI(ref Unitportrait p)
    {
        //if(Minions_List_BG != null)
        //{
        //    this.gameObject.GetComponent<Image>().sprite = Minions_List_BG;
        //}
        if (p.CampText != null)
        {
            p.CampText.text = pro_e_Camp ;
        }
        if (p.ClassText != null)
        {
            p.ClassText.text = pro_Minion_ClassName;
        }
        if (p.MinionName != null)
        {
            p.MinionName.text = pro_Minion_e_Name;
        }
        if(p.LevelText != null)
        {
            p.LevelText.text = pro_MinionLv.ToString();
        }

        if (p.gameObject.tag != "Untagged")
        {
            switch (p.gameObject.tag)
            {
                case "Minions_Inventory":
                    p.gameObject.GetComponent<Image>().sprite = Level_Inventory[(int)pro_UnitRank];
                    break;
                case "Minions_EditList":
                    p.gameObject.GetComponent<Image>().sprite = EditBGSprite;
                    break;
                case "Minions_Standing":
                    //p.gameObject.GetComponent<Image>().sprite = Level_Standing[(int)pro_UnitRank];
                    break;
                default:
                    break;
            }
        }

        if (p.ClassFrame != null )
        {
            p.ClassFrame.gameObject.SetActive(true);
            switch (pro_Minion_ClassName)
            {
                case "Guardian":
                    p.ClassFrame.sprite = ClassSprite[(int)MINION_CLASS.CLASS_GUARDIAN];
                    break;
                case "Rescue":
                    p.ClassFrame.sprite = ClassSprite[(int)MINION_CLASS.CLASS_RESCUE];
                    break;
                case "Mage":
                    ClassFrame.sprite = ClassSprite[(int)MINION_CLASS.CLASS_MAGE];
                    break;
                case "Buster":
                    p.ClassFrame.sprite = ClassSprite[(int)MINION_CLASS.CLASS_BUSTER];
                    break;
                case "Chaser":
                    p.ClassFrame.sprite = ClassSprite[(int)MINION_CLASS.CLASS_CHASER];
                    break;
                case "Paladin":
                    p.ClassFrame.sprite = ClassSprite[(int)MINION_CLASS.CLASS_PALADIN];
                    break;
                default:
                    break;
            }
        }
        if (p.LevelFrame != null)
        {
            p.LevelFrame.gameObject.SetActive(true);
            p.LevelFrame.sprite = Level_Sprite[(int)pro_UnitRank];
        }
        if (p.StarImage != null)
        {
            p.StarImage.gameObject.SetActive(true);
            p.StarImage.sprite = RankStar[(int)pro_UnitRank];
        }

       
    }
    public void GetData(ref Unitportrait p)
    {
        //Unitportrait p = new Unitportrait();
        p.pro_Atk = pro_Atk;
        p.pro_AtSpeed = pro_AtSpeed;
        p.pro_Critical = pro_Critical;
        p.pro_Cost = pro_Cost;
        p.pro_Def = pro_Def;
        p.pro_getCount = pro_getCount;
        p.pro_Hp = pro_Hp;
        p.pro_MinionLv = pro_MinionLv;
        p.pro_Minion_ClassName = pro_Minion_ClassName;
        p.pro_Minion_e_Name = pro_Minion_e_Name;
        p.pro_Minion_k_Name = pro_Minion_k_Name;
        p.pro_RelocationTimer = pro_RelocationTimer;
        p.pro_ReturnTimer = pro_ReturnTimer;
        p.pro_UnitRank = pro_UnitRank;
        UpdateUI(ref p);
        //p.GetComponent<Image>().sprite = pro_MinionSprite;

    }
    //public Sprite pro_MinionSprite
    //{
    //    get
    //    {
    //        return UnitSprite;
    //    }
    //    set
    //    {
    //        UnitSprite = value;
    //        gameObject.GetComponent<Image>().sprite = UnitSprite;
    //    }
    //}

    public string pro_Minion_k_Name
    {
        get
        {
            return Minion_k_Name;
        }
        set
        {
            Minion_k_Name = value;
        }

    }
    public string pro_Minion_e_Name
    {
        get
        {
            return Minion_e_Name;
        }
        set
        {
            Minion_e_Name = value;
        }
    }
    
    public string pro_e_Camp
    {
        get
        {
            return e_camp;
        }
        set
        {
            e_camp = value;
        }
    }
    public string pro_Minion_ClassName
    {
        get
        {
            return Minion_ClassName;
        }
        set
        {
            Minion_ClassName = value;
        }
    }

    public int pro_MinionLv
    {
        get
        {
            return Minioun_Lv;
        }
        set
        {
            Minioun_Lv = value ;
        }
    }
    public RANK pro_UnitRank
    {
        get
        {
            return MInion_Rank;
        }

        set
        {
            MInion_Rank = value;
        }
    }
    public int pro_getCount
    {
        get
        {
            return getCount;
        }

        set
        {
            getCount = value;
        }
    }

    public int pro_Cost
    {
        get
        {
            return Cost;
        }

        set
        {
            Cost = value;
        }
    }

    public float pro_ReturnTimer
    {
        get
        {
            return ReturnTimer;
        }
        set
        {
            ReturnTimer = value;
        }
    }
    
    public float pro_RelocationTimer
    {
        get
        {
            return RelocationTimer;
        }
        set
        {
            RelocationTimer = value;
        }
    }

    public float pro_Hp
    {
        get
        {
            return Hp;
        }
        set
        {
            Hp = value;
        }
    }

    public float pro_AtSpeed
    {
        get
        {
            return AtSpeed;
        }
        set
        {
            AtSpeed = value;
        }
    }

    public float pro_Atk
    {
        get
        {
            return Atk;
        }
        set
        {
            Atk = value;
        }
    }
    public float pro_Def
    {
        get
        {
            return Def;
        }
        set { Def = value; }
    }

    public float pro_Critical
    {
        get
        {
            return Critical;
        }
        set
        {
            Critical = value;
        }
    }






}
