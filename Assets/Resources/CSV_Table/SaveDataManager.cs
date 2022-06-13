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
    public int 
        Level;
    public int VogueLevel;
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
    private static SaveDataManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static SaveDataManager getInstance()
    {
        return instance;
    }

    public void newGame()
    {
        SaveData data = new SaveData();

        data.Name = "";
        data.Crystal = 0;
        data.VertyLevel = 0;
        data.IsabellaLevel = 0;
        data.EilgosLevel = 0;
        data.ZippoLevel = 0;
        data.KuenLevel = 0;
        //data.PardoLevel = 0;
        data.WratihLevel = 0;
        data.HwaseonLevel = 0;
        data.AsherLevel = 0;
        data.PayLevel = 0;
        data.SophiaLevel = 0;
        data.AngelusLevel = 0;
        data.NoahLevel = 0;
        data.EilgosLevel = 0;

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
