using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 본 클래스는 캐릭터를 추가, 삭제, 정렬하는 클래스임을 명시. 
public class UnitList : MonoBehaviour, IPointerDownHandler, IBeginDragHandler,IEndDragHandler,IDragHandler
{
    private bool SelectUnit = false;
    private GameObject Dummy;
    private Vector2 MousePos;
    private RaycastHit hit;
    private GraphicRaycaster gr;
    Canvas myCanves;
    Camera myCamera;
    private void Start()
    {
        myCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        myCanves = GameObject.Find("Canvas").GetComponent<Canvas>();
        gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
    }
    private void Update()
    {

    }

    public void SetEditting(Unitportrait up)
    {
        if (EditList.Instance.ListCheck(Dummy.GetComponent<Unitportrait>()))
        {
            up.pro_UnitName = Dummy.GetComponent<Unitportrait>().pro_UnitName;
            up.pro_UnitLv = 10;
            up.pro_UnitRank = 10;
            up.pro_getCount = 10;
            up.pro_UnitImage = Dummy.GetComponent<Image>().sprite;
        }
        else
        {
            Debug.Log("같은 캐릭터를 추가할 수 없습니다.");
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
        //throw new System.NotImplementedException();
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
                //CreateDummy(results[0].gameObject.GetComponent<Collider2D>());
                SetEditting(results[0].gameObject.GetComponent<Unitportrait>());
                //Debug.Log("cost");
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        try
        {
            Debug.Log("드래그");
            //if(Input.MouseDra)
            if (SelectUnit)
            {
                MousePos = Input.mousePosition;
                //MousePos = camera.ScreenToWorldPoint(MousePos);

                Dummy.transform.position = MousePos;
                //Debug.Log("활성화");
            }
        }
        catch (System.NotImplementedException e)
        {
            Debug.Log(e.Message);
        }
        //throw new System.NotImplementedException();
    }
}
