using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Unit_Select_UI : MonoBehaviour
{
    public GameObject[] MinionButton;
    public GameObject[] SkillButton;
    public bool[] UnitOnOff;
    public GameObject TurretAktCircle;

    int Button_interval =165; //카드 간격
    int Unit_Maximum_Number = 12;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Unit_Maximum_Number; i++)
        {
            //MinionButton[i].GetComponent<MinionButton>().UnitIndex = MinionButton[i].GetComponent<MinionButton>().UnitIndex + 1;
            int UnitIndex = MinionButton[i].GetComponent<MinionButton>().UnitIndex;
            MinionButton[i].transform.DOLocalMoveX(100 + Button_interval * UnitIndex, 0.5f, false); 
        }
    }

    public void Reset() //배치시간
    {
        TurretAktCircle.SetActive(false);
        for (int i = 0; i < Unit_Maximum_Number; i++)
        {
            if (UnitOnOff[i] == false)
            {
                int UnitIndex = MinionButton[i].GetComponent<MinionButton>().UnitIndex;
                MinionButton[i].transform.DOLocalMoveY(47,0.5f,false);
                MinionButton[i].transform.DOLocalMoveX(120 + Button_interval * UnitIndex, 0.5f, false);

            }
            else
            {
                int UnitIndex = MinionButton[i].GetComponent<MinionButton>().UnitIndex;
                MinionButton[i].transform.DOLocalMoveX(120 + Button_interval * UnitIndex, 0.5f, false);
            }
        }
        
    }

    public void SkillReset() //전투시간
    {

        int UnitIndex = 0;
        TurretAktCircle.SetActive(true);
        for (int i = 0; i < Unit_Maximum_Number; i++)
        {
           
            if (UnitOnOff[i] == true)
            {
                SkillButton[i].transform.DOLocalMoveY(47, 0.5f, false);
                SkillButton[i].transform.DOLocalMoveX(230 + Button_interval * UnitIndex, 0.5f, false);
                UnitIndex++;
                Debug.Log(SkillButton[i].name+UnitIndex);
            }
            else
            {
                SkillButton[i].transform.DOLocalMoveX(230 + Button_interval * UnitIndex, 0.5f, false);
            }
        }   

        for(int i = 0; i < Unit_Maximum_Number; i++)
        {
            if (UnitOnOff[i] == true)
            {
                int nameIndex = 0;
                for (int j = 0; j < Unit_Maximum_Number; j++)
                {
                    if (UnitOnOff[j] == true)
                    {
                        if (SkillButton[i].GetComponent<SkillButton>().NameIndex > SkillButton[j].GetComponent<SkillButton>().NameIndex)
                        {
                            nameIndex++;
                        }
                    }

                }
                SkillButton[i].GetComponent<SkillButton>().CurrntIndex = nameIndex;
            }
        }

    }

    void moveButtonLeft(int character_Number)
    {
        MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex-1;
        int UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex;
        MinionButton[character_Number].transform.DOLocalMoveX(120+Button_interval * UnitIndex, 0.5f,false);
    }
    void moveButtonRight(int character_Number)
    {
        MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex + 1;
        int UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex;
        MinionButton[character_Number].transform.DOLocalMoveX(120+Button_interval * UnitIndex, 0.5f, false);


    }
    public void DownButton(int character_Number)
    {
        MinionButton[character_Number].transform.DOLocalMoveY(-150, 0.3f);
    }

    public void UpButton(int character_Number)
    { 
        MinionButton[character_Number].transform.DOLocalMoveY(50, 0.3f);

    }

    public void Hide_Unit_Button(int character_Number)
    {
        DownButton(character_Number);

        for (int i = character_Number+1; i < Unit_Maximum_Number; i++)
        {
            moveButtonLeft(i);
        }
    }
    public void Display_Unit_Button(int character_Number)
    {
        UpButton(character_Number);

        for (int i = character_Number+1; i < Unit_Maximum_Number; i++)
        {
            moveButtonRight(i);
        }
    }

    public void Hide_Unit_Button(string character_Name)
    {
        int Character_Number = -1;
        switch (character_Name)
        {
            case "Verity":
                Character_Number = 0;
                break;
            case "Zippo":
                Character_Number = 1;
                break;
            case "Asher":
                Character_Number = 2;
                break;
            case "Pay":
                Character_Number = 3;
                break;
            case "Isabella":
                Character_Number = 4;
                break;
            case "Kuen":
                Character_Number = 5;
                break;
            case "Pardo":
                Character_Number = 6;
                break;
            case "Sophia":
                Character_Number = 7;
                break;
            case "Vogue":
                Character_Number = 8;
                break;
            case "Eremedium":
                Character_Number = 9;
                break;
            case "Wraith":
                Character_Number = 10;
                break;
            case "Hwaseon":
                Character_Number = 11;
                break;
            default:
                break;

        }
        DownButton(Character_Number);
        UnitOnOff[Character_Number] = true;
        for (int i = Character_Number + 1; i < Unit_Maximum_Number; i++)
        {
            moveButtonLeft(i);
        }
    }

    public void Display_Unit_Button(string character_Name)
    {
        int Character_Number = -1;
        switch (character_Name)
        {
            case "Verity":
                Character_Number = 0;
                break;
            case "Zippo":
                Character_Number = 1;
                break;
            case "Asher":
                Character_Number = 2;
                break;
            case "Pay":
                Character_Number = 3;
                break;
            case "Isabella":
                Character_Number = 4;
                break;
            case "Kuen":
                Character_Number = 5;
                break;
            case "Pardo":
                Character_Number = 6;
                break;
            case "Sophia":
                Character_Number = 7;
                break;
            case "Vogue":
                Character_Number = 8;
                break;
            case "Eremedium":
                Character_Number = 9;
                break;
            case "Wraith":
                Character_Number = 10;
                break;
            case "Hwaseon":
                Character_Number = 11;
                break;
            default:
                break;

        }

        UpButton(Character_Number);
        UnitOnOff[Character_Number] = false;
        for (int i = Character_Number + 1; i < Unit_Maximum_Number; i++)
        {
            moveButtonRight(i);
        }
    }



    // Update is called once per frame
    void Update()
    {


    }
}
