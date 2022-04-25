using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : Singleton<MinionManager>
{
    public List<GameObject> minionPrefabs = new List<GameObject>();
    public List<Minion> minionQueue = new List<Minion>();

    void Start()
    {
        foreach(var hero in minionPrefabs)
        {
            minionQueue.Add(hero.GetComponent<Minion>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
