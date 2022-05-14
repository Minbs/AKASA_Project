using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tooltip tooltip;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(true);
        tooltip.SetupTooltip("[�׽�Ʈ Ŭ���� �̸�]", "�׽�Ʈ Ŭ����", "�׽�Ʈ Ŭ���� ����", "3 / 4 / 5");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
