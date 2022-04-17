using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class MapCreator
{
    // Start is called before the first frame update

    

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        InitTiles();
        ReadTilesInfo();
        BoardManager.Instance.isTileSet = true;
    }

    void InitTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach(var tile in tiles)
        {
            if(tile.GetComponent<Tile>() == null)
            {
              tile.AddComponent<Tile>();
            }

            if (tile.GetComponent<BoxCollider>() == null)
            {
                tile.AddComponent<BoxCollider>();
            }

            BoardManager.Instance.tilesList.Add(tile.GetComponent<Tile>());
  
        }


     var result = BoardManager.Instance.tilesList.OrderBy(x=> Mathf.Ceil( x.transform.position.z) / 100).ThenByDescending(x=> x.transform.position.x); // Ÿ�� ��ġ �������� ����Ʈ ����
        BoardManager.Instance.tilesList = result.ToList();

        
    }

    void ReadTilesInfo()
    {
        string textFile = Resources.Load<TextAsset>("Datas/TileInfo_Stage1") .text;
        //   StringReader stringReader = new StringReader(textFile.text);

        //    Debug.Log(stringReader.ReadToEnd());
        StringReader stringReader = new StringReader(textFile);

        int tileCount = 0;
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();

            if (line == null)
                break;

            string[] s = line.Split(' ');

            switch (s[0])
            {
                case "Size":
                    BoardManager.Instance.sizeX = int.Parse(s[1]);
                    BoardManager.Instance.sizeY = int.Parse(s[2]);
                    break;

                case "Tile":
                    BoardManager.Instance.tilesList[tileCount].SetTileInfo(s); //pos, isEmpty ����
                    tileCount++;
                    break;

                default:
                    break;
            }
        }

        // Close Text File
        stringReader.Close();

    }


}
