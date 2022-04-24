using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public List<GameObject> effectsList;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void InstantiateAttackEffect(string effectName, Vector3 pos)
    {
        GameObject attackEffect = null;
        foreach (var e in effectsList)
        {
            if (e.name == effectName)
            {
                attackEffect = e;
                break;
            }
        }


        GameObject effect = Instantiate(attackEffect);
        effect.transform.position = pos;
        Destroy(effect, 3);
    }

    public IEnumerator InstantiateHomingEffect(string effectName, GameObject target, float duration)
    {
        GameObject attackEffect = null;
        foreach (var e in effectsList)
        {
            if (e.name == effectName)
            {
                attackEffect = e;
                break;
            }
        }


        GameObject effect = Instantiate(attackEffect);

        float timer = 0f;


        while (timer < duration)
        {
            if (target == null || !target.activeSelf || target.GetComponent<Unit>().currentHp <= 0)
            {
                Destroy(effect);
                break;
            }


            timer += Time.deltaTime;
            effect.transform.position = target.transform.position;
            yield return null;
        }



        Destroy(effect);
    }
}
