using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class Wavedata
{
    public float Stage;
    public float Wave;
    public float WaveReward;
    public float MonsterReward;
    public float MonsterNum;
    public float WaveTotalReward;
    public float Total;
}

public class CSV_WaveData_Reader : MonoBehaviour
{
    public Wavedata WaveData1_1;
    public Wavedata WaveData1_2;
    public Wavedata WaveData1_3;
    public Wavedata WaveData1_4;
    public Wavedata WaveData1_5;
    public Wavedata WaveData1_6;
    public Wavedata WaveData1_7;
    public Wavedata WaveData1_8;
    public Wavedata WaveData1_9;
    public Wavedata WaveData1_10;




    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> WaveDataList = CSVReader.Read("LevelDesignDataList.xlsx - StageWaveReward");

        StageWaveUpdate(WaveData1_1,WaveDataList,0);
        StageWaveUpdate(WaveData1_2, WaveDataList, 1);
        StageWaveUpdate(WaveData1_3, WaveDataList, 2);
        StageWaveUpdate(WaveData1_4, WaveDataList, 3);
        StageWaveUpdate(WaveData1_5, WaveDataList, 4);
        StageWaveUpdate(WaveData1_6, WaveDataList, 5);
        StageWaveUpdate(WaveData1_7, WaveDataList, 6);
        StageWaveUpdate(WaveData1_8, WaveDataList, 7);
        StageWaveUpdate(WaveData1_9, WaveDataList, 8);
        StageWaveUpdate(WaveData1_10, WaveDataList, 9);



    }

    void StageWaveUpdate(Wavedata wavedata, List<Dictionary<string, object>> WaveDataList,int StageNum)
    {
        wavedata.Stage = float.Parse(WaveDataList[StageNum]["Stage"].ToString());
        wavedata.Wave = float.Parse(WaveDataList[StageNum]["Wave"].ToString());
        wavedata.WaveReward = float.Parse(WaveDataList[StageNum]["WaveReward"].ToString());
        wavedata.MonsterReward = float.Parse(WaveDataList[StageNum]["MonsterReward"].ToString());
        wavedata.WaveTotalReward = float.Parse(WaveDataList[StageNum]["WaveTotalReward"].ToString());
        wavedata.Total = float.Parse(WaveDataList[StageNum]["Total"].ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
