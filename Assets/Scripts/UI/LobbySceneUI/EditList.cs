using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditList : Singleton<EditList>
{
    // GraphicRaycaster gr;
    // 이 곳은 파티 편집기에 등록한 파티를 저장하고 초기화하는 기능
    public List<Unitportrait> myList = new List<Unitportrait>();
    
    public int EditMax = 10;
    public Unitportrait EditPanelPrefab;
    // Start is called before the first frame update
    void Start()
    {
        EditMax = 10;
        if (EditMax / 2 == 1)       // 무조건 짝수로 변환해줘야 함.
            EditMax++;
        for(int i = 0; i < EditMax; i++)
        {
            Unitportrait go = Instantiate<Unitportrait>(EditPanelPrefab);

            myList.Add(go);

            go.transform.parent = this.transform;
        }
    }

    public void ListClear()
    {
        EditMax = 10;
        if (EditMax / 2 == 1)       // 무조건 짝수로 변환해줘야 함.
            EditMax++;
        
        for(int i = 0;  i < myList.Count;i++)
        {
            Destroy(myList[i].gameObject);
        }
        myList.Clear();
        
        for (int i = 0; i < EditMax; i++)
        {
            Unitportrait go = Instantiate<Unitportrait>(EditPanelPrefab);

            myList.Add(go);

            go.transform.parent = this.transform;
        }
    }

    public bool ListCheck(Unitportrait up)
    {
        foreach (Unitportrait minions in myList)
        {
            if (minions.pro_Minion_k_Name == up.pro_Minion_k_Name)    // 추후 일련번호로 수정
                return false;               // Panel 띄워주기
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
