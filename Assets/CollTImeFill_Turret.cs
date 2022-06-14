using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollTImeFill_Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Turret;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Image>().fillAmount = Turret.GetComponent<Turret>().attackTimer / Turret.GetComponent<Turret>().attackSpeed;
    }
}