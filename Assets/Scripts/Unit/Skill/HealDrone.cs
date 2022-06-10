using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDrone : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target { get; set; }

    private float timer;

    private float healTerm;
    public float duration { get; set; }

    public float healRange { get; set; }
    public float healAmount { get; set; }

    void Start()
    {
        timer = 0;
        healTerm = 1;

        StartCoroutine(HealTargets());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * GameManager.Instance.gameSpeed;

        if (!target.activeSelf || timer > duration)
            Destroy(gameObject);
    }

    IEnumerator HealTargets()
    {
        EffectManager.Instance.InstantiateHomingEffect("ground_heal", target, duration);

        float healTermTimer = 0;

        while (timer <= duration)
        {

            yield return null;


            healTermTimer += Time.deltaTime * GameManager.Instance.gameSpeed;
            Collider[] colliders = Physics.OverlapSphere(target.transform.position, healRange);
            if (healTermTimer >= healTerm)
            {
                foreach (var col in colliders)
                {
                    if (col.transform.parent.GetComponent<Minion>())
                    {
                        col.transform.parent.GetComponent<Unit>().Deal(-healAmount);
                    }
                }

                healTermTimer = 0;
            }


        }
    }
}
