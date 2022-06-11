using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [Header("페이지")]
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
            Debug.Log($"선택된 페이지 넘버: {number}, 0 < {FuncPage.Count} 사이의 숫자를 입력해 주세요.");
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
