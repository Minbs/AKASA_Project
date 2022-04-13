using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject attackEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateAttackEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(attackEffect);
        effect.transform.position = pos;
        Destroy(effect, 5);
    }
}
