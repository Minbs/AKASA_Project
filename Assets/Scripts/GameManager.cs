using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum State
{
    WAIT, //���� ���� ��
    BATTLE, //���� ����
    END //���� ����
}

public class GameManager : Singleton<GameManager>
{
    public int cost = 20;

    public float waitTimer = 20; // ��� �ð�

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
