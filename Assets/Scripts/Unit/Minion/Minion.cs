using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinionClass
{
    //근거리 클래스
    Buster,
    Guardian,

    //원거리 클래스
    Chaser,
    Rescue
}

public class Minion : Unit
{
    public MinionClass minionClass;


    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        
    }
    private void OnMouseUp()
    {
        if (GameManager.Instance.state == State.WAIT && GameManager.Instance.deployState == DeployState.NONE)
        {
            if (Input.GetMouseButtonUp(0))
                GameManager.Instance.minionChangePos(gameObject);
            else if (Input.GetMouseButtonUp(1))
                BattleUIManager.Instance.SetMinionUpgradeUI(gameObject);
        }
    }

    
  
}
