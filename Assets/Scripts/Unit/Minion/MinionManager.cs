using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : Singleton<MinionManager>
{
    public List<GameObject> heroPrefabs = new List<GameObject>();
    public List<DefenceMinion> heroQueue = new List<DefenceMinion>();

    void Start()
    {
        foreach(var hero in heroPrefabs)
        {
            heroQueue.Add(hero.GetComponent<DefenceMinion>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
