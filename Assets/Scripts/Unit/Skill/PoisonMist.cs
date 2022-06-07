using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMist : MonoBehaviour
{
    public float damage = 1;
    public float duration = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.transform.parent.GetComponent<Unit>().GetPoisoned(damage, duration);

            Debug.Log(other.transform.parent.name);
        }
    }
}
