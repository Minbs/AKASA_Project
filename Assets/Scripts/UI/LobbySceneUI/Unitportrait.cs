using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Unitportrait : MonoBehaviour
{
    public Sprite UnitImage;
    public string UnitName;
    public int UnitLv;
    public int UnitRank; // enum이 존재할 경우 enum으로 변경
    public int getCount; // 획득 날짜.


    // 임시로 값을 설정
    private void Start()
    {
        UnitImage = this.gameObject.GetComponent<Image>().sprite;

    }

    public Sprite pro_UnitImage
    {
        get
        {
            return UnitImage;
        }
        set
        {
            UnitImage = value;
            gameObject.GetComponent<Image>().sprite = UnitImage;
        }
    }
    public string pro_UnitName
    {
        get
        {
            return UnitName;
        }
        set
        {
            UnitName = value;
        }

    }
    public int pro_UnitLv
    {
        get
        {
            return UnitLv;
        }
        set
        {
            UnitLv = value ;
        }
    }
    public int pro_UnitRank
    {
        get
        {
            return UnitRank;
        }

        set
        {
            UnitRank = value;
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





}
