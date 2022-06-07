using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Unit_Select_UI : MonoBehaviour
{
    public GameObject[] MinionButton;
    public bool[] UnitOnOff;
    bool UnitToggle1 = false;            //시연 버그 방지용 토글
    bool UnitToggle2 = false;
    bool UnitToggle3 = false;
    bool UnitToggle4 = false;
    bool UnitToggle5 = false;
    bool UnitToggle6 = false;
    bool UnitToggle7 = false;
    bool UnitToggle8 = false;
    bool UnitToggle9 = false;

    int Button_interval =165; //카드 간격
    int Unit_Maximum_Number = 9;

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

    public void Reset()
    {
        Debug.Log("reset");
        for(int i = 0; i < Unit_Maximum_Number; i++)
        {
            if (UnitOnOff[i] == true)
            {
                int UnitIndex = MinionButton[i].GetComponent<MinionButton>().UnitIndex;
                MinionButton[i].transform.DOLocalMoveX(100 + Button_interval * UnitIndex, 0.5f, false);
            }
            else
            {
                int UnitIndex = MinionButton[i].GetComponent<MinionButton>().UnitIndex;
                MinionButton[i].transform.DOLocalMoveX(100 + Button_interval * UnitIndex, 0.5f, false);
                //MinionButton[i].transform.DOLocalMoveY(-350, 0.3f);
            }
        }
        
    }
    void moveButtonLeft(int character_Number)
    {
        MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex-1;
        int UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex;
        MinionButton[character_Number].transform.DOLocalMoveX(100+Button_interval * UnitIndex, 0.5f,false);
    }
    void moveButtonRight(int character_Number)
    {
        MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex + 1;
        int UnitIndex = MinionButton[character_Number].GetComponent<MinionButton>().UnitIndex;
        MinionButton[character_Number].transform.DOLocalMoveX(100+Button_interval * UnitIndex, 0.5f, false);


    }
    public void DownButton(int character_Number)
    {
        Debug.Log("버튼 다운");
        MinionButton[character_Number].transform.DOLocalMoveY(-350, 0.3f);
    }

    public void UpButton(int character_Number)
    {
        Debug.Log("버튼 업");
        MinionButton[character_Number].transform.DOLocalMoveY(-150, 0.3f);

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
                Debug.Log("Hide");
                Hide_Unit_Button(0);
                UnitToggle1 = true;
                UnitOnOff[0] = true;
            }
            else
            {
                Debug.Log("Display");
                Display_Unit_Button(0);
                UnitToggle1 = false;
                UnitOnOff[0] = false;

            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (UnitToggle2 == false)
            {
                Hide_Unit_Button(1);
                UnitToggle2 = true;
                UnitOnOff[1] = true;
            }
            else
            {
                Display_Unit_Button(1);
                UnitToggle2 = false;
                UnitOnOff[1] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (UnitToggle3 == false)
            {
                Hide_Unit_Button(2);
                UnitToggle3 = true;
                UnitOnOff[2] = true;
            }
            else
            {
                Display_Unit_Button(2);
                UnitToggle3 = false;
                UnitOnOff[2] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (UnitToggle4 == false)
            {
                Hide_Unit_Button(3);
                UnitToggle4 = true;
                UnitOnOff[3] = true;
            }
            else
            {
                Display_Unit_Button(3);
                UnitToggle4 = false;
                UnitOnOff[3] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (UnitToggle5 == false)
            {
                Hide_Unit_Button(4);
                UnitToggle5 = true;
                UnitOnOff[4] = true;
            }
            else
            {
                Display_Unit_Button(4);
                UnitToggle5 = false;
                UnitOnOff[4] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (UnitToggle6 == false)
            {
                Hide_Unit_Button(5);
                UnitToggle6 = true;
                UnitOnOff[5] = true;
            }
            else
            {
                Display_Unit_Button(5);
                UnitToggle6 = false;
                UnitOnOff[5] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (UnitToggle7 == false)
            {
                Hide_Unit_Button(6);
                UnitToggle7 = true;
                UnitOnOff[6] = true;
            }
            else
            {
                Display_Unit_Button(6);
                UnitToggle7 = false;
                UnitOnOff[6] = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (UnitToggle8 == false)
            {
                Hide_Unit_Button(7);
                UnitToggle8 = true;
                UnitOnOff[7] = true;
            }
            else
            {
                Display_Unit_Button(7);
                UnitToggle8 = false;
                UnitOnOff[7] = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (UnitToggle9 == false)
            {
                Hide_Unit_Button(8);
                UnitToggle9 = true;
                UnitOnOff[8] = true;
            }
            else
            {
                Display_Unit_Button(8);
                UnitToggle9 = false;
                UnitOnOff[8] = false;
            }
        }


    }
}
