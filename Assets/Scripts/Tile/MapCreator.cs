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
        GameManager.Instance.isTileSet = true;
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

            GameManager.tilesList.Add(tile.GetComponent<Tile>());
  
        }


     var result = GameManager.tilesList.OrderBy(x=> x.transform.position.z).ThenByDescending(x=> x.transform.position.x); // Ÿ�� ��ġ �������� ����Ʈ ����
        GameManager.tilesList = result.ToList();
    }

    void ReadTilesInfo()
    {
        string path = Application.dataPath;
        path += "/stage1.txt";

        string[] lines = System.IO.File.ReadAllLines(path);

        if (lines.Length <= 0)
            return;

        int tileCount = 0;

        for(int i = 0; i < lines.Length; i++)
        {
            string[] s = lines[i].Split(' ');

            switch(s[0])
            {
                case "Size":
                    GameManager.Instance.sizeX = int.Parse(s[1]);
                    GameManager.Instance.sizeY = int.Parse(s[2]);
                    break;

                case "Tile":
                    GameManager.tilesList[tileCount].SetTileInfo(s); //pos, isEmpty ����
                    tileCount++;
                    break;

                default:
                    break;
            }
        }
    }


}
