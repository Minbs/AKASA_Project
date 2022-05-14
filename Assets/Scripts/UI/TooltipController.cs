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
        tooltip.SetupTooltip("[테스트 클래스 이름]", "테스트 클래스", "테스트 클래스 설명", "3 / 4 / 5");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
