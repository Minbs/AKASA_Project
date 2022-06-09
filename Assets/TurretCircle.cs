using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretCircle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;

        if (this.transform.position.y < 200f)
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
    }
}
