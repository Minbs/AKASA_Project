using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : Singleton<MinionManager>
{
    public List<GameObject> heroPrefabs = new List<GameObject>();
    public List<Minion> heroQueue = new List<Minion>();
    public List<float> heroTime = new List<float>();

    void Start()
    {
        foreach(var hero in heroPrefabs)
        {
            heroQueue.Add(hero.GetComponent<Minion>());
            heroTime.Add(hero.GetComponent<float>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
