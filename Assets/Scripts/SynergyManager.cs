using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : Singleton<SynergyManager>
{
    public List<Node> guardianSynergyNodes = new List<Node>();
    public List<Node> bursterSynergyNodes = new List<Node>();
    public List<Node> chaserSynergyNodes = new List<Node>();
    public List<Node> rescueSynergyNodes = new List<Node>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckClassSynergy()
    {
        if (GameManager.Instance.minionsList.Count <= 0)
            return;

       MinionClass minionClass = MinionClass.Buster;
       foreach (var minion in GameManager.Instance.minionsList)
       {    
         switch(minionClass)
            {
                case MinionClass.Buster:
                    break;
                case MinionClass.Guardian:
                    break;
                case MinionClass.Chaser:
                    break;
                case MinionClass.Rescue:
                    break;
            }
       }
    }

    void ActiveGuardianSynergy()
    {

    }

    void ActiveChaserSynergy()
    {

    }

    void ActiveRescueSynergy()
    {

    }
}
