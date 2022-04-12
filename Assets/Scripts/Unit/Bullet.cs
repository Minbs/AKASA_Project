using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private int damage;
    private GameObject target;
    
    

    void Start()
    {
        
    }

    public void Init(int damage, GameObject target)
    {
        this.damage = damage;
        this.target = target;

        Vector2 camPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 targetCamPos = Camera.main.WorldToScreenPoint(target.transform.position);

        Vector2 diff = targetCamPos
    - camPos;
        diff.Normalize();



        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion qt = Quaternion.AngleAxis(rot_z, Vector3.forward);
        Debug.Log(qt.eulerAngles);
        transform.eulerAngles = qt.eulerAngles;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + 116.438f, transform.eulerAngles.y, transform.eulerAngles.z + 90);
    }
    // Update is called once per frame
    void Update()
    {
     

        Vector3 des = target.transform.position;
        
       // des.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, des, speed * Time.deltaTime);

       
    }
}
