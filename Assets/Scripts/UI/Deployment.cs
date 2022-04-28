using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deployment : MonoBehaviour
{
    public GameObject deploy;
    Image deployRange;
    bool isCheck;
    Vector3 min, max;
    [SerializeField]
    float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        deployRange = deploy.GetComponentInChildren<Image>();

        min = new Vector3(0.1f, 0.1f, 0.1f);
        max = new Vector3(5.0f, 5.0f, 1.0f);
        deployRange.transform.localScale = min;
        deploy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        deploy.SetActive(isCheck);

        if (deploy.activeSelf)
            deployRange.transform.localScale = Vector3.Lerp(min, max, speed * Time.deltaTime);
        else
            deployRange.transform.localScale = min;
    }

    public void OnDeployButtonDown() => isCheck = deploy.activeSelf == true ? false : true;
    public void OnNum1ButtonDown() => Debug.Log("Num1 Button Down");
    public void OnNum2ButtonDown() => Debug.Log("Num2 Button Down");
    public void OnNum3ButtonDown() => Debug.Log("Num3 Button Down");
    public void OnNum4ButtonDown() => Debug.Log("Num4 Button Down");
}
