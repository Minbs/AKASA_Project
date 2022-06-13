using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float maxHp;
    public float currentHp;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Deal(float damage)
    {
        if (GetComponent<Unit>())
            GetComponent<Unit>().Deal(damage);
        else if (GetComponent<Turret>())
            GetComponent<Turret>().Deal(damage);
    }
}
