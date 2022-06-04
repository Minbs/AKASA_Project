using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Unit_Select_UI : MonoBehaviour
{
    public GameObject[] MinionButton;
    bool UnitToggle1 = false;            //�ÿ� ���� ������ ���
    bool UnitToggle2 = false;
    bool UnitToggle3 = false;
    bool UnitToggle4 = false;

    int Button_interval = 267;
    int Unit_Maximum_Number = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void moveButtonLeft(int character_Number)
    {
        MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex-1;
        int UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex;
        MinionButton[character_Number].transform.DOMoveX(521 + Button_interval*UnitIndex, 1f, false);
    }
    void moveButtonRight(int character_Number)
    {
        MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex + 1;
        int UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex;
        MinionButton[character_Number].transform.DOMoveX(521 + Button_interval * UnitIndex, 1f, false);


    }
    void tempMove()
    {
        MinionButton[0].transform.DOMoveX(0, 1f, false);

    }
    public void DownButton(int character_Number)
    {
        MinionButton[character_Number].transform.DOMoveY(-77, 0.5f);
    }

    public void UpButton(int character_Number)
    {
        MinionButton[character_Number].transform.DOMoveY(133, 0.5f);
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


    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (UnitToggle1 == false)
            {
                Hide_Unit_Button(0);
                UnitToggle1 = true;
            }
            else
            {
                Display_Unit_Button(0);
                UnitToggle1 = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (UnitToggle2 == false)
            {
                Hide_Unit_Button(1);
                UnitToggle2 = true;
            }
            else
            {
                Display_Unit_Button(1);
                UnitToggle2 = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (UnitToggle3 == false)
            {
                Hide_Unit_Button(2);
                UnitToggle3 = true;
            }
            else
            {
                Display_Unit_Button(2);
                UnitToggle3 = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (UnitToggle4 == false)
            {
                Hide_Unit_Button(3);
                UnitToggle4 = true;
            }
            else
            {
                Display_Unit_Button(3);
                UnitToggle4 = false;
            }
        }

    }
}
