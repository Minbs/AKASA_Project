using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerityShot : MonoBehaviour
{
    public GameObject target { get; set; }

    public float damage { get; set; }

    public float speed { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Vector2 camPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 targetCamPos = Camera.main.WorldToScreenPoint(target.transform.position);

        Vector2 diff = targetCamPos
    - camPos;
        diff.Normalize();



        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion qt = Quaternion.AngleAxis(rot_z, Vector3.forward);

        transform.eulerAngles = qt.eulerAngles;
        transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= 0.01f)
        {
                target.GetComponent<Unit>().Deal(damage);
                Destroy(gameObject);
        }

        Vector3 des = target.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, des, speed * Time.deltaTime);
    }
}
