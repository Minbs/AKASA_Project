using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public List<GameObject> effectsList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateAttackEffect(string effectName,Vector3 pos)
    {
        GameObject attackEffect = null;
        foreach (var e in effectsList)
        {
            if(e.name == effectName)
            {
                attackEffect = e;
                break;
            }
        }

        
        GameObject effect = Instantiate(attackEffect);
        effect.transform.position = pos;
        Destroy(effect, 3);
    }
}
