using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum State
{
    WAIT, //전투 시작 전
    BATTLE, //전투 진행
    END //전투 종료
}

public class GameManager : Singleton<GameManager>
{
    public int cost = 20;

    public float waitTimer = 20; // 대기 시간

    public State state;

    public EnemySpawner spawner;

    void Start()
    {
        state = State.WAIT;
    }

    void Update()
    {
        if (waitTimer <= 0 && state == State.WAIT)
        {
            state = State.BATTLE;
            StartCoroutine(spawner.Spawn());
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }
    }

    
}
