using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject logoObj;
    [SerializeField]
    private List<GameObject> logoImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private GameObject InformObj;
    [SerializeField]
    private List<GameObject> InformImage;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI explanationText;

    private float halfWidth;
    RectTransform rt;

    private void Start()
    {
        halfWidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        rt = GetComponent<RectTransform>();

        logoObj = transform.GetChild(0).gameObject;
        for (int i = 0; i < logoObj.transform.childCount; i++)
            logoImage.Add(logoObj.transform.GetChild(i).gameObject);

        nameText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        InformObj = transform.GetChild(3).gameObject;
        for (int i = 0; i < InformObj.transform.childCount; i++)
            InformImage.Add(InformObj.transform.GetChild(i).gameObject);

        titleText = transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        explanationText = transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();

        for (int i = 0; i < logoObj.transform.childCount; i++)
            if (logoImage[i].gameObject.activeSelf)
                logoImage[i].gameObject.SetActive(false);

        for (int i = 0; i < InformObj.transform.childCount; i++)
            if (InformImage[i].gameObject.activeSelf)
                InformImage[i].gameObject.SetActive(false);

        if (this.gameObject.activeSelf)
            this.gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;

        if (rt.anchoredPosition.x + rt.sizeDelta.x > halfWidth)
            rt.pivot = new Vector2(1, 1);
        else
            rt.pivot = new Vector2(0, 1);
    }

    public void SetupTooltip(string nameTxt, string titleTxt, string explanationTxt)
    {
        for (int i = 0; i < logoObj.transform.childCount; i++)
            if (logoImage[i].gameObject.activeSelf)
                logoImage[i].gameObject.SetActive(false);

        for (int i = 0; i < InformObj.transform.childCount; i++)
            if (InformImage[i].gameObject.activeSelf)
                InformImage[i].gameObject.SetActive(false);

        nameText.text = nameTxt;
        switch (nameTxt)
        {
            case "Guardian":
                logoImage[0].gameObject.SetActive(true);
                InformImage[0].gameObject.SetActive(true);
                break;
            case "Rescue":
                logoImage[1].gameObject.SetActive(true);
                InformImage[1].gameObject.SetActive(true);
                break;
            case "Buster":
                logoImage[2].gameObject.SetActive(true);
                InformImage[2].gameObject.SetActive(true);
                break;
            case "Chaser":
                logoImage[3].gameObject.SetActive(true);
                InformImage[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
        titleText.text = titleTxt;
        explanationText.text = explanationTxt;
    }
}
