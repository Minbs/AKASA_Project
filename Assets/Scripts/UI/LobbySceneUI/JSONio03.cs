using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

[Serializable]
public class MainObjectData<T>
{
    public T[] highscore;
    //publi
}

[Serializable]
public class InnerObjectData
{
    public string name;
    public int scores;
}

public class JSONio03 : MonoBehaviour
{
    public MainObjectData<InnerObjectData> mainObject;
    public InnerObjectData innerObject;

    List<InnerObjectData> objectList = new List<InnerObjectData>();

    public InnerObjectData createSubObject(string name, int scores)
    {
        InnerObjectData myInnerObject = new InnerObjectData();
        myInnerObject.name = name;
        myInnerObject.scores = scores;
        return myInnerObject;
    }

    private void Start()
    {
        objectList.Add(createSubObject("BadBoy", 8828));
        objectList.Add(createSubObject("Madmax", 4711));

        mainObject.highscore = objectList.ToArray();

        string generatedJsonString = JsonUtility.ToJson(mainObject);

        //File.WriteAllText(Application.dataPath + "/file122.json", generatedJsonString);
    }
}


