using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI explanationText;
    public TextMeshProUGUI numberText;
    private float halfWidth;
    RectTransform rt;

    public void SetupTooltip(string nameTxt, string classTxt, string explanationTxt, string numberTxt)
    {
        nameText.text = nameTxt;
        classText.text = classTxt;
        explanationText.text = explanationTxt;
        numberText.text = numberTxt;
    }

    private void Start()
    {
        halfWidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;

        if (rt.anchoredPosition.x + rt.sizeDelta.x > halfWidth)
            rt.pivot = new Vector2(1, 1);
        else
            rt.pivot = new Vector2(0, 1);
    }
}
