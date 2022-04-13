using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// �� Ŭ������ ĳ���͸� �߰�, ����, �����ϴ� Ŭ�������� ���. 
public class UnitList : Singleton<UnitList>, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool SelectUnit = false;
    private GameObject Dummy;
    private Vector2 MousePos;
    private RaycastHit hit;
    private GraphicRaycaster gr;
    [SerializeField] private List<Unitportrait> MinionList;
    [SerializeField] private List<Unitportrait> temp = new List<Unitportrait>();
    [SerializeField] private bool SortType;         // true == ��������, false == ��������
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
                Debug.Log("���� ĳ���͸� �߰��� �� �����ϴ�.");
            }
        }
        catch
        {
            Debug.Log("Editor Setting Error");
        }
    }

    public void CreateDummy(Collider2D other)
    {
        Debug.Log("Ȱ��ȭ");
        if (!SelectUnit)
        {
            SelectUnit = true;
            GameObject a = other.gameObject;
            Dummy = Instantiate<GameObject>(a, Input.mousePosition, Quaternion.identity);
            Dummy.GetComponent<RectTransform>().sizeDelta = new Vector2(125, 125);
            Dummy.transform.parent = myCanves.transform;

            // ������ ���İ� ����
        }
    }

    private void DestroyDummy()
    {
        Debug.Log("��Ȱ��ȭ");
        if (SelectUnit)
        {
            Destroy(Dummy);
            SelectUnit = false;

            // ������ alpha �� ����
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        try
        {
            Debug.Log("�����ʹٿ�");
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
            Debug.Log("���巡��");
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
            Debug.Log("���� �巡��");
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
            Debug.Log("�巡��");
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
    // ���� Sort
    public void SortingName()       // �̸����� ����
    {
        try
        {
            MinionList.Sort((MinionA, MinionB) => MinionA.pro_Minion_e_Name.CompareTo(MinionB.pro_Minion_e_Name));   // �̸� ��?
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

    public void SortingLv()         // ������ ����
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

    public void SortingRank()       // ������� ����
    {

    }

    public void SortingGetTime()    // �Լ��Ϸ� ����
    {

    }

}
