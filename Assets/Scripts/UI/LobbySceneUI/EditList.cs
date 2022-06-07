using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;

using System.IO;

public class EditList : Singleton<EditList>
{
    // GraphicRaycaster gr;
    // 이 곳은 파티 편집기에 등록한 파티를 저장하고 초기화하는 기능
    public List<MinionsPortrait> myList = new List<MinionsPortrait>();
    public List<MinionsPortrait> objList = new List<MinionsPortrait>();
    public GameObject Contents;
    public int EditMax = 10;
    //public MainObjectDataa ArrayItem = new MainObjectDataa();
    public MinionsPortrait EditPanelPrefab;

    MinionsPortrait dummy;

    public List<string> e_NameList;

    private void Awake()
    {

    }

    public GameObject pro_Contents
    {
        get
        {
            if (Contents == null)
                Contents = GameObject.Find("EditContent");
            return Contents;
        }
        set
        {
            //SetList();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (myList.Count == 0)
        {
            SetEdit();
        }
    }

    public void SetEdit()
    {
        myList.Clear();
        EditMax = 10;
        if (EditMax / 2 == 1)       // 무조건 짝수로 변환해줘야 함.
            EditMax++;
        for (int i = 0; i < EditMax; i++)
        {
        //    MinionsPortrait go = Instantiate<MinionsPortrait>(EditPanelPrefab);

        //    myList.Add(go);

         //   go.transform.parent = pro_Contents.transform;
        }
        // 여기서 로드!
        //LoadJson();
    }

    public void ListClear()
    {
        EditMax = 10;
        if (EditMax / 2 == 1) // 무조건 짝수로 변환해줘야 함.
            EditMax++;
        
        for(int i = 0;  i < myList.Count;i++)
        {
            Destroy(myList[i].gameObject);
        }
        myList.Clear();
        
        for (int i = 0; i < EditMax; i++)
        {
            MinionsPortrait go = Instantiate<MinionsPortrait>(EditPanelPrefab);

            myList.Add(go);
            go.transform.parent = this.transform;
        }
    }

    public bool ListCheck(MinionsPortrait up)
    {
        foreach (MinionsPortrait minions in myList)
        {
            if (minions.pro_k_name == up.pro_k_name)    // 추후 일련번호로 수정
                return false;               // Panel 띄워주기
        }
        return true;
    }
}
