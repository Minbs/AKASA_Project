using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
// 본 클래스는 캐릭터를 추가, 삭제, 정렬하는 클래스임을 명시. 
public class UnitList : Singleton<UnitList>, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool SelectUnit = false;
    private string UnitListFile = "myUnits";    // 불러올 파일 이름
    private List<string> myUnits = new List<string>();
    private GameObject Dummy;
    private Vector2 MousePos;
    private RaycastHit hit;
    private GraphicRaycaster gr;
    [SerializeField] private List<Unitportrait> MinionList;
    [SerializeField] private Unitportrait Prefab;
    [SerializeField] private List<Unitportrait> temp;
    [SerializeField] public List<Sprite> Minions_Illust;
    [SerializeField] public List<Sprite> Minions_Standing;
    Canvas myCanves;
    Camera myCamera;
    private void Start()
    {
        myCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        myCanves = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();

        ListSave();
        //DontDestroyOnLoad(this.gameObject);
        //ListLoad();
        //LoadList();
    }
    public void ListLoad()
    {

    }

    public void ListSave()
    {
        string path;
        string filename;
        string json;
        for (int i = 0; i < MinionList.Count; i++)
        {
            filename = MinionList[i].pro_Minion_e_Name;
            path = Application.dataPath + "/" + filename + ".Json";

            json = JsonUtility.ToJson(MinionList[i]);

            File.WriteAllText(path, json);
            Debug.Log(filename + "생성");
            myUnits.Add(filename);
        }
        path = Application.dataPath + "/" + UnitListFile + ".Json";
        string[] a = myUnits.ToArray();
        json = JsonUtility.ToJson(a);
        File.WriteAllText(path, json);
    }

    // 리스트 랜덤 불러오기
    //private void LoadList()
    //{
    //    // 임시로 10개 받아오기
    //    for(int i = 0; i < 10; i++)
    //    {
    //        Unitportrait up = Instantiate<Unitportrait>(Prefab);

    //        int a = Random.Range(0, Minions_Illust.Count);
    //        switch (a)
    //        {
    //            case 0:
    //                up.RandInit("eremedium", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 1:
    //                up.RandInit("kuen", Minions_Illust[a], Minions_Standing[a]);

    //                break;
    //            case 2:
    //                up.RandInit("verity", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 3:
    //                up.RandInit("wraith", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 4:
    //                up.RandInit("zippo", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 5:
    //                up.RandInit("kuen", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 6:
    //                up.RandInit("kuen", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 7:
    //                up.RandInit("kuen", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 8:
    //                up.RandInit("kuen", Minions_Illust[a], Minions_Standing[a]);
    //                break;
    //            case 9:
    //                break;
    //            case 10:
    //                break;
    //            default:
    //                break;
    //        }

    //        MinionList.Add(up);

    //        up.transform.parent = this.transform;
            
    //    }
    //}

    private void Update()
    {

    }

    public void SetEditting(Unitportrait up)
    {

        try
        {
            if (EditList.Instance.ListCheck(Dummy.GetComponent<Unitportrait>()))
            {
                Dummy.GetComponent<Unitportrait>().GetData(ref up);
                //EditList.Instance.e_NameList.Add(up.pro_Minion_e_Name);
                Unitportrait dummy = up;
                //up.GetData(ref dummy);
                EditList.Instance.objList.Add(dummy);
            }
            else
            {
                Debug.Log("같은 캐릭터를 추가할 수 없습니다.");
            }
        }
        catch
        {
            Debug.Log("Editor Setting Error");
        }

    }

    public void CreateDummy(Collider2D other)
    {
        Debug.Log("활성화");
        if (!SelectUnit)
        {
            SelectUnit = true;
            GameObject a = other.gameObject;
            Dummy = Instantiate<GameObject>(a, Input.mousePosition, Quaternion.identity);
            Dummy.GetComponent<RectTransform>().sizeDelta = new Vector2(125, 125);
            Dummy.transform.parent = myCanves.transform;
        }
    }

    private void DestroyDummy()
    {
        Debug.Log("비활성화");
        if (SelectUnit)
        {
            Destroy(Dummy);
            SelectUnit = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        try
        {
            Debug.Log("포인터다운");
        }
        catch(System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        try
        {
            Debug.Log("비긴드래그");
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count <= 0) return;
            for(int i = 0; i  < results.Count; i++)
            {
                if (results[i].gameObject.tag == "Minions_Inventory")
                {
                    CreateDummy(results[i].gameObject.GetComponent<Collider2D>());
                    break;
                }
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            DestroyDummy();
            Debug.Log("엔드 드래그");
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count <= 0) return;
            if (results[0].gameObject.tag == "Minions_EditList")
            {
                SetEditting(results[0].gameObject.GetComponent<Unitportrait>());
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        try
        {
            Debug.Log("드래그");
            if (SelectUnit)
            {
                MousePos = Input.mousePosition;

                Dummy.transform.position = MousePos;
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
    }


    [SerializeField] private bool SortType;         // true == 오름차순, false == 내림차순
    public void SortReverse()
    {
        if (SortType)
            MinionList.Reverse();
    }
    // 정렬 Sort
    public void SortingName()       // 이름으로 정렬
    {
        try
        {
            MinionList.Sort((MinionA, MinionB) => MinionA.pro_Minion_e_Name.CompareTo(MinionB.pro_Minion_e_Name));   // 이름 순?
            SortReverse();
        }
        catch
        {
            Debug.Log("Sorting Error : Name");
        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            Unitportrait g = Instantiate<Unitportrait>(MinionList[i]);

            temp.Add(g);

            g.transform.parent = this.transform;

        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            Destroy(MinionList[i].gameObject);
        }


        MinionList = temp;

        temp = new List<Unitportrait>();

    }

    public void SortingLv()         // 레벨로 정렬
    {
        try
        {
            MinionList.Sort((MinionA, MinionB) => MinionA.pro_MinionLv.CompareTo(MinionB.pro_MinionLv));
            SortReverse();
        }
        catch
        {
            Debug.Log("Sorting Error : Lv");
        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            Unitportrait g = Instantiate<Unitportrait>(MinionList[i]);

            temp.Add(g);

            g.transform.parent = this.transform;

        }

        for (int i = 0; i < MinionList.Count; i++)
        {
            Destroy(MinionList[i].gameObject);
        }


        MinionList = temp;

        temp = new List<Unitportrait>();
    }

    public void SortingRank()       // 등급으로 정렬
    {

    }

    public void SortingGetTime()    // 입수일로 정렬
    {

    }


}
