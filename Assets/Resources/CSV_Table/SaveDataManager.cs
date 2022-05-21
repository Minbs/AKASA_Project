using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class SaveData
{
    public string Name;
    public int Crystal;
    public int VertyLevel;
    public int IsabellaLevel;
    public int EremediumLevel;
    public int ZippoLevel;
    public int KuenLevel;
    public int PardoLevel;
    public int WratihLevel;
    public int HwaseonLevel;
    public int AsherLevel;
    public int PayLevel;
    public int SophiaLevel;
    public int AngelusLevel;
    public int NoahLevel;
    public int EilgosLevel;

}

public class SaveDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void newGame()
    {
        SaveData data = new SaveData();

        data.Name = "";
        data.Crystal = 1;
        data.VertyLevel = 0;
        data.IsabellaLevel = 1;
        data.EilgosLevel = 1;
        data.ZippoLevel = 1;
        data.KuenLevel = 1;
        data.PardoLevel = 1;
        data.WratihLevel = 1;
        data.HwaseonLevel = 1;
        data.AsherLevel = 1;
        data.PayLevel = 1;
        data.SophiaLevel = 1;
        data.AngelusLevel = 1;
        data.NoahLevel = 1;
        data.EilgosLevel = 1;

        File.WriteAllText(Application.dataPath + "/SaveData.json", JsonUtility.ToJson(data));
    }


    // Start is called before the first frame update
    void Start()
    {
        newGame();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
