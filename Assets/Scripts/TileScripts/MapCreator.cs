using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class MapCreator : MonoBehaviour
{
    // Start is called before the first frame update

    int tileColumnCount;
    int tileRowCount;

    GameObject tilePrefab;

    

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
        GameManager.instance.isTileSet = true;
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


     var result = GameManager.tilesList.OrderBy(x=> x.transform.position.z).ThenByDescending(x=> x.transform.position.x);
        GameManager.tilesList = result.ToList();

        foreach (var tile in GameManager.tilesList)
       {
         //Debug.Log(tile.transform.position);
        }
    }

    void ReadTilesInfo()
    {
        string path = Application.dataPath;
        path += "/stage1.txt";

        string[] lines = System.IO.File.ReadAllLines(path);

        if (lines.Length <= 0)
            return;

        for(int i = 0; i < lines.Length; i++)
        {
            string[] s = lines[i].Split(' ');
            //     Debug.Log(GameManager.TilesList.Count);
            GameManager.tilesList[i].SetTileInfo(s); //pos, isEmpty ¼³Á¤
        }
    }


}
