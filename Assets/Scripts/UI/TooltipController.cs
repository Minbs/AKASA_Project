using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string synergyObj;
    public Tooltip tooltip;

    public void GetTooltip(string synergy)
    {
        switch (synergy)
        {
            case "Guardian":
                tooltip.SetupTooltip(synergy, "[가디언 클래스]", 
                    "<color=red>가디언 클래스</color> 기준으로\n" +
                    "적용 범위에 위치한 유닛은\n" +
                    "받는 피해가 "+ SynergyManager.Instance.guardianCoefficient+ "% 감소한다.");
                break;
            case "Rescue":
                tooltip.SetupTooltip(synergy, "[레스큐 클래스]",
                    "<color=red>레스큐 클래스</color> 기준으로\n" +
                    "적용 범위에 위치한 유닛은\n" +
                    "받는 힐링양이 " + SynergyManager.Instance.rescueCoefficient + "%증가한다.");
                break;
            case "Buster":
                tooltip.SetupTooltip(synergy, "[버스터 클래스]",
                    "<color=red>버스터 클래스</color> 기준으로\n" +
                    "상하 1칸에 배치 된 아군 수마다 \n" +
                    "공격력 " + SynergyManager.Instance.busterCoefficient + "(" + SynergyManager.Instance.busterCoefficient * 2 + ")%\n" +
                    "체력" + SynergyManager.Instance.busterCoefficient + "(" + SynergyManager.Instance.busterCoefficient * 2 + ")%" + "증가.");
                break;
            case "Chaser":
                tooltip.SetupTooltip(synergy, "[체이서 클래스]", 
                    "<color=red>체이서 클래스</color> 기준으로\n" +
                    "상하 2칸에 배치된 아군 수마다\n" +
                    "공격속도" + +SynergyManager.Instance.chaserCoefficient + "(" + SynergyManager.Instance.busterCoefficient*2 + ")%\n");
                break;
            default:
                tooltip.SetupTooltip(synergy, "[테스트 클래스 이름]", "<color=red>테스트 클래스</color> 테스트 클래스 설명");
                break;
        }

    }

    public void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GetTooltip(synergyObj);
        tooltip.gameObject.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
