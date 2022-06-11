using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [Header("������")]
    public List<GameObject> FuncPage;


    [Space(10f)]
    [Header("ButtonSprite")]
    public Sprite onSprite;
    public Sprite offSprite;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ChangeSettingPage(int number)
    {
        if(number >= FuncPage.Count || number <= 0)
        {
            Debug.Log($"���õ� ������ �ѹ�: {number}, 0 < {FuncPage.Count} ������ ���ڸ� �Է��� �ּ���.");
            return;
        }
        for(int i = 0; i < FuncPage.Count; i++)
        {
            if(i == number - 1)
            {
                FuncPage[i].SetActive(true);
            }
            else
            {
                FuncPage[i].SetActive(false);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
