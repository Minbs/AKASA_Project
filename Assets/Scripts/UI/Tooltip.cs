using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
 //   [SerializeField]
   // private TextMeshProUGUI nameText;
 //   [SerializeField]
  //  private TextMeshProUGUI titleText;
   // [SerializeField]
   // private TextMeshProUGUI explanationText;

    private Sprite synergyIconSprite;

    [Header("시너지 아이콘 이미지")]
    public Sprite guardianSynergyIcon;
    public Sprite chaserSynergyIcon;
    public Sprite busterSynergyIcon;
    public Sprite rescueSynergyIcon;

 //   private float halfWidth;
    RectTransform rt;

    private void Start()
    {
     //   halfWidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        rt = GetComponent<RectTransform>();

      //  nameText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

   //     InformObj = transform.GetChild(3).gameObject;

     //   titleText = transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
    //    explanationText = transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();

        //시너지 왼쪽 아이콘 이미지
        synergyIconSprite = transform.GetChild(0).GetComponent<Image>().sprite;
    }

    private void Update()
    {
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, Camera.main, out point);
        rt.localPosition = point;

    //    if (rt.anchoredPosition.x + rt.sizeDelta.x > halfWidth)
    //        rt.pivot = new Vector2(1, 1);
    //    else
      //      rt.pivot = new Vector2(0, 1);
    }

    public void SetupTooltip(string nameTxt, string titleTxt, string explanationTxt)
    {
        //nameText.text = nameTxt;
        switch (nameTxt)
        {
            case "Guardian":
                synergyIconSprite = guardianSynergyIcon;
                break;
            case "Rescue":
                synergyIconSprite = rescueSynergyIcon;
                break;
            case "Buster":
                synergyIconSprite = busterSynergyIcon;
                break;
            case "Chaser":
                synergyIconSprite = chaserSynergyIcon;
                break;
            default:
                break;
        }
        //titleText.text = titleTxt;
    //    explanationText.text = explanationTxt;
    }
}
