using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform root;

    void Start()
    {
        root = transform.root;
    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        root.BroadcastMessage("BeginDrag", transform, SendMessageOptions.DontRequireReceiver);

    }

    public void OnDrag(PointerEventData eventData)
    {
        //canvas의 Render Mode를 overlay가 아닌 camera로 하면 오류남
        transform.position = eventData.position;

        root.BroadcastMessage("Drag", transform, SendMessageOptions.DontRequireReceiver);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        root.BroadcastMessage("EndDrag", transform, SendMessageOptions.DontRequireReceiver);
    }
}
