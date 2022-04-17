using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;

using System.IO;

public class EditList : Singleton<EditList>
{
    public class MainObjectDataa
    {
        public SaveLoadData[] highscore;
    }   

    // GraphicRaycaster gr;
    // 이 곳은 파티 편집기에 등록한 파티를 저장하고 초기화하는 기능
    public List<Unitportrait> myList = new List<Unitportrait>();
    public List<Unitportrait> objList = new List<Unitportrait>();
    public GameObject Contents;
    public int EditMax = 10;
    public MainObjectDataa ArrayItem = new MainObjectDataa();
    public Unitportrait EditPanelPrefab;

    Unitportrait dummy;

    public List<string> e_NameList;

    private void Awake()
    {
        //Debug.Log()
        //if (Instance != null)
        //{ 
        //    DontDestroyOnLoad(this.gameObject);
        //}
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
        else
        {

        }
        //if (EditList.Instance == this)
        //{
        //    SetEdit();
        //}
        //else
        //{
        //    Instance.SetEdit();
        //    Destroy(this.gameObject);
        //}
    }

    public void SetEdit()
    {
        myList.Clear();
        EditMax = 10;
        if (EditMax / 2 == 1)       // 무조건 짝수로 변환해줘야 함.
            EditMax++;
        for (int i = 0; i < EditMax; i++)
        {
            Unitportrait go = Instantiate<Unitportrait>(EditPanelPrefab);

            myList.Add(go);

            go.transform.parent = pro_Contents.transform;
        }
        // 여기서 로드!
        LoadJson();
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

    public void CreateJson()
    {
        string fileName = "Party1";
        string path = Application.dataPath + "/" + fileName + ".json";
        if (!File.Exists(path))
        {
            using(File.Create(path))
            {
                Debug.Log(fileName + "파일 생성 성공!");
            }
        }
        else
        {
            Debug.Log(fileName + "파일이 이미 존재합니다.");
        }
    }


    public void SaveBinary()
    {

    }
    public void SaveJsonFile()
    {
        //MainObjectData<SaveLoadData> mainObject = new MainObjectData<SaveLoadData>();
        //string filename = "Party";
        //string path = Application.dataPath + "/" + filename + ".json";

        //List<SaveLoadData> items = new List<SaveLoadData>();
        ////CreateJson();

        //for (int i = 0; i < myList.Count; i++)
        //{
        //    Unitportrait up = EditList.Instance.myList[i];
        //    if (up.pro_Minion_e_Name != "")
        //    {
        //        SaveLoadData a = new SaveLoadData(up.pro_Minion_e_Name);
        //        items.Add(a);
        //    }
        //}


        ////mainObject.highscore = items.ToArray();
        //string json = JsonConvert.SerializeObject(items.ToArray());

        ////string json = JsonSerializer.Serialize<List>
        //File.WriteAllText(path, json);
    }

    public void LoadJsonFile()
    {
    }
    
    public void SaveJson()
    {
    }

    public void LoadJson()
    {
        //try
        //{
        //    for(int i =0; i < e_NameList.Count; i++)
        //    {
        //        string path = Application.dataPath + "/" + e_NameList[i] + ".Json";
        //        if(File.Exists(path))
        //        {
        //            string json = File.ReadAllText(path);
        //            Debug.Log(json);
        //            Unitportrait t = JsonUtility.FromJson<Unitportrait>(json);
        //            //t.GetData(ref myList[i].GetComponent<Unitportrait>());
        //            myList[i]
        //        }

        //    }
        //}



    }


    public class SaveLoadData
    {
        public string e_name;
        public SaveLoadData()
        {
            e_name = string.Empty;
        }
        public SaveLoadData(string _e_name)
        {
            e_name = _e_name;
        }

        public string To_Json()
        {
            return JsonUtility.ToJson(this);
        }

    }

}
