using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RANK
{
    RANK_RARE,
    RANK_EPIC,
    RANK_LEGEND
}
public class Unitportrait : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private Sprite UnitSprite;
    [SerializeField] private Text CampText;
    [SerializeField] private Text ClassText;
    [SerializeField] private Text MinionName;
    [SerializeField] private Text LevelText;

    [Header("Rank Sprite")]
    [SerializeField] private List<Sprite> Level_frame;
    [SerializeField] private List<Sprite> RankStar;

    [Header("Unit Class")]
    [SerializeField] private List<Sprite> ClassSprite;


    [SerializeField] private int Minion_num;
    [Header("이름")]
    [SerializeField]  private string Minion_k_Name;
    [SerializeField] private string Minion_e_Name;
    [Space (10f)]
    [Header("클래스")]
    [SerializeField] private string Minion_ClassName;
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
        
        UnitSprite = this.gameObject.GetComponent<Image>().sprite;

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
        p.GetComponent<Image>().sprite = pro_MinionSprite;

    }
    public Sprite pro_MinionSprite
    {
        get
        {
            return UnitSprite;
        }
        set
        {
            UnitSprite = value;
            gameObject.GetComponent<Image>().sprite = UnitSprite;
        }
    }
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
