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

    public void InstantiateHomingEffect(string effectName, GameObject target, float duration)
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
        effect.AddComponent<Effect>();
        StartCoroutine(effect.GetComponent<Effect>().HomingEffect(effectName, target, duration));
    }

}
