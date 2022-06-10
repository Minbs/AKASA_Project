using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : Singleton<UserData>
{
    // 본 스크립트는 유저가 저장해야 할 데이터를 가지고 있는 스크립트데아루.

    [SerializeField]
    private string userName = "나인테일즈";

    [Space(10f)]
    [Header("유저가 소유 중인 미니언 데이터")]
    [SerializeField]
    private List<MinionsInfo> MinionData;
    [Space(10f)]
    [Header("인게임 재화")]
    [SerializeField]
    private int coin;
    [SerializeField]
    private int ether;

    void Start()
    {
        if (UserData.Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //Instance.SetEdit();
            Destroy(this.gameObject);
        }
    }

    public void SaveUnitData(List<MinionsPortrait> portrait)
    {
        MinionData.Clear();
        int m_count = portrait.Count;
        for(int i = 0; i < m_count; i++)
        {
            MinionData.Add(portrait[i].pro_info);
        }
        Debug.Log("데이터 저장");
    }

    public List<MinionsInfo> LoadUnitData()
    {
        return MinionData;
    }
}
