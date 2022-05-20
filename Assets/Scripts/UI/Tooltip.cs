using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI explanationText;
    public TextMeshProUGUI numberText;
    private float halfWidth;
    RectTransform rt;

    private void Start()
    {
        halfWidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        rt = GetComponent<RectTransform>();

        nameText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        explanationText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        numberText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();

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

    public void SetupTooltip(string nameTxt, string explanationTxt, string numberTxt)
    {
        nameText.text = nameTxt;
        explanationText.text = explanationTxt;
        numberText.text = numberTxt;
    }
}
