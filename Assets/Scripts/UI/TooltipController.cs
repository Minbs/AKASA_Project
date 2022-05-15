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
        tooltip.SetupTooltip("[�׽�Ʈ Ŭ���� �̸�]", "<color=red>�׽�Ʈ Ŭ����</color> �׽�Ʈ Ŭ���� ����", "<color=orange>3</color> / 4 / 5");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
