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

    int Button_interval =165; //ī�� ����
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

    public void Reset() //��ġ�ð�
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

    public void SkillReset() //�����ð�
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

            }
            else
            {
                //int UnitIndex = SkillButton[i].GetComponent<MinionButton>().UnitIndex;
                SkillButton[i].transform.DOLocalMoveX(230 + Button_interval * UnitIndex, 0.5f, false);
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
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (UnitOnOff[0] == false)
            {
                Hide_Unit_Button(0);
                UnitOnOff[0] = true;
 
            }
            else
            {
                Display_Unit_Button(0);
                UnitOnOff[0] = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (UnitOnOff[1] == false)
            {
                Hide_Unit_Button(1);
                UnitOnOff[1] = true;

            }
            else
            {
                Display_Unit_Button(1);
                UnitOnOff[1] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (UnitOnOff[2] == false)
            {
                Hide_Unit_Button(2);
                UnitOnOff[2] = true;

            }
            else
            {
                Display_Unit_Button(2);
                UnitOnOff[2] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (UnitOnOff[3] == false)
            {
                Hide_Unit_Button(3);
                UnitOnOff[3] = true;
            }
            else
            {
                Display_Unit_Button(3);
                UnitOnOff[3] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (UnitOnOff[4] == false)
            {
                Hide_Unit_Button(4);
                UnitOnOff[4] = true;

            }
            else
            {
                Display_Unit_Button(4);
                UnitOnOff[4] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (UnitOnOff[5] == false)
            {
                Hide_Unit_Button(5);
                UnitOnOff[5] = true;

            }
            else
            {
                Display_Unit_Button(5);
                UnitOnOff[5] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (UnitOnOff[6] == false)
            {
                Hide_Unit_Button(6);
                UnitOnOff[6] = true;
            }
            else
            {
                Display_Unit_Button(6);
                UnitOnOff[6] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (UnitOnOff[7] == false)
            {
                Hide_Unit_Button(7);
                UnitOnOff[7] = true;

            }
            else
            {
                Display_Unit_Button(7);
                UnitOnOff[7] = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (UnitOnOff[8] == false)
            {
                Hide_Unit_Button(8);
                UnitOnOff[8] = true;

            }
            else
            {
                Display_Unit_Button(8);
                UnitOnOff[8] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (UnitOnOff[9] == false)
            {
                Hide_Unit_Button(9);
                UnitOnOff[9] = true;

            }
            else
            {
                Display_Unit_Button(9);
                UnitOnOff[9] = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (UnitOnOff[10] == false)
            {
                Hide_Unit_Button(10);
                UnitOnOff[10] = true;

            }
            else
            {
                Display_Unit_Button(10);
                UnitOnOff[10] = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (UnitOnOff[11] == false)
            {
                Hide_Unit_Button(11);
                UnitOnOff[11] = true;

            }
            else
            {
                Display_Unit_Button(11);
                UnitOnOff[11] = false;
            }
        }


    }
}
