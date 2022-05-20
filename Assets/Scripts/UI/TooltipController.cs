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
        tooltip.SetupTooltip("[테스트 클래스 이름]", "<color=red>테스트 클래스</color> 테스트 클래스 설명", "<color=orange>3</color> / 4 / 5");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
