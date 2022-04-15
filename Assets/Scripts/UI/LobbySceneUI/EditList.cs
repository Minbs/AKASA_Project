using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

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
        for (int i = 0; i < EditMax; i++)
        {
            Unitportrait go = Instantiate<Unitportrait>(EditPanelPrefab);

            myList.Add(go);

            go.transform.parent = this.transform;
        }
        // 여기서 로드!

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
    
    public void Save()
    {
        try
        {
            
            string fileName = "Party1";
            string path = Application.dataPath + "/" + fileName + ".Json";

            List<SaveLoadData> items = new List<SaveLoadData>();
            for(int i = 0; i < myList.Count; i++)
            {
                Unitportrait up = EditList.Instance.myList[i];
                if(up.pro_Minion_e_Name != "")
                {
                    SaveLoadData save = new SaveLoadData(up.pro_Minion_num, up.pro_Minion_k_Name,
                        up.pro_Minion_e_Name, up.pro_Minion_ClassName, up.pro_e_Camp, up.pro_NameTag,
                        up.pro_ClassSimbol, up.pro_UnitRank );

                    items.Add(save);
                    Debug.Log(save.e_name + "저장 중...");
                }
            }

            string json = JsonScript.ToJson(items);

            File.WriteAllText(path, json);
        }
        catch
        {
            Debug.Log("SaveError");
        }
    }


    public class SaveLoadData
    {
        // 유닛 넘버
        public int minion_Num;
        // 한글 이름
        public string k_name;
        // 영어 이름
        public string e_name;
        // 클래스
        public string className;
        // 레벨?
        // 진영
        public string camp;
        // 이름 태그
        public UnitName nameTag;
        // 클래스 심볼
        public MINION_CLASS ClassSimbol;
        // 등급
        public RANK RankLevel;
        public SaveLoadData(int num, string _k_name, string _e_name,
            string _className, string _camp, UnitName _nameTag,
            MINION_CLASS _ClassSimbol, RANK _RankLevel)
        {
            minion_Num = num;
            k_name = _k_name;
            e_name = _e_name;
            className = _className;
            camp = _camp;
            nameTag = _nameTag;
            ClassSimbol = _ClassSimbol;
            RankLevel = _RankLevel;
        }

    }

}
