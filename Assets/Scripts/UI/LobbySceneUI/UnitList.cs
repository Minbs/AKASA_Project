using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Unitportrait Prefab;
    [SerializeField] public List<Sprite> Minions_Illust;
    [SerializeField] public List<Sprite> Minions_Standing;
    Canvas myCanves;
    Camera myCamera;
    private void Start()
    {
        myCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        myCanves = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        //LoadList();
    }

    // ����Ʈ ���� �ҷ�����
    //private void LoadList()
    //{
    //    // �ӽ÷� 10�� �޾ƿ���
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
        }
    }

    private void DestroyDummy()
    {
        Debug.Log("��Ȱ��ȭ");
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
    }



}
