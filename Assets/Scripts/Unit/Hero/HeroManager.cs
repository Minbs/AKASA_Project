using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : Singleton<HeroManager>
{
    public List<GameObject> heroPrefabs = new List<GameObject>();
    public List<Hero> heroQueue = new List<Hero>();


    void Start()
    {
        foreach(var hero in heroPrefabs)
        {
            heroQueue.Add(hero.GetComponent<Hero>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
