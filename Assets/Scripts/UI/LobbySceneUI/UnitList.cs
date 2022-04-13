using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 본 클래스는 캐릭터를 추가, 삭제, 정렬하는 클래스임을 명시. 
public class UnitList : Singleton<UnitList>, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool SelectUnit = false;
    private GameObject Dummy;
    private Vector2 MousePos;
    private RaycastHit hit;
    private GraphicRaycaster gr;
    [SerializeField] private List<Unitportrait> MinionList;
    [SerializeField] private List<Unitportrait> temp = new List<Unitportrait>();
    [SerializeField] private bool SortType;         // true == 오름차순, false == 내림차순
    Canvas myCanves;
    Camera myCamera;
    private void Start()
    {
        SortType = false;
        myCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        myCanves = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
    }
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

            // 원본의 알파값 변경
        }
    }

    private void DestroyDummy()
    {
        Debug.Log("비활성화");
        if (SelectUnit)
        {
            Destroy(Dummy);
            SelectUnit = false;

            // 원본의 alpha 값 변경
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
            if (results[0].gameObject.tag == "Minions_Inventory")
            {
                CreateDummy(results[0].gameObject.GetComponent<Collider2D>());
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
        //throw new System.NotImplementedException();
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
        //throw new System.NotImplementedException();
    }


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

        for(int i = 0; i < MinionList.Count; i++)
        {
            Destroy(MinionList[i].gameObject);
        }


        MinionList = temp;

        temp = new List<Unitportrait>() ;

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
