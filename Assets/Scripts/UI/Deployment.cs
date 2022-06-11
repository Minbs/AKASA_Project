using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deployment : MonoBehaviour
{
    public GameObject deploy;
    Image deployRange;
    bool isCheck;
    float time = 0, rotate = 360;

    [SerializeField]
    int rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        deployRange = deploy.GetComponentInChildren<Image>();

        SizeReset();
        deploy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheck)
            SizeBigger();
        else
            SizeSmaller();
    }

    public void OnDeployButtonDown() => isCheck = deploy.activeSelf == true ? false : true;
    public void OnNum1ButtonDown() => Debug.Log("Num1 Button Down");
    public void OnNum2ButtonDown() => Debug.Log("Num2 Button Down");
    public void OnNum3ButtonDown() => Debug.Log("Num3 Button Down");
    public void OnNum4ButtonDown() => Debug.Log("Num4 Button Down");

    public void SizeBigger()
    {
        deploy.SetActive(isCheck);

        if (time >= 1f)
        {
            time = 1; 
            return;
        }
        
        deployRange.transform.localScale = Vector3.one * (1 * time);
        deployRange.transform.Rotate(0, 0, rotate * rotateSpeed * Time.deltaTime);
        time += Time.deltaTime;
    }

    public void SizeSmaller()
    {
        if (time <= 0f)
        {
            time = 0;
            deploy.SetActive(isCheck);
            return;
        }
        deployRange.transform.localScale = Vector3.one * (1 * time);
        deployRange.transform.Rotate(0, 0, -rotate * rotateSpeed * Time.deltaTime);
        time -= Time.deltaTime;
    }

    public void SizeReset()
    {
        time = 0;
        deployRange.transform.localScale = Vector3.one;
        deployRange.transform.rotation = Quaternion.identity;
    }
}
