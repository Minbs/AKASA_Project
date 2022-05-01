using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField]
    public List<Tile> tilesList = new List<Tile>();

    public Vector3 tileStartPos;

    public int sizeX;
    public int sizeY;


    public bool isTileSet = false;

    public bool end = false;

    private MapCreator mapCreator;


    // Start is called before the first frame update
    void Awake()
    {
        mapCreator = new MapCreator();

        if (tilesList == null)
            Debug.Log("n");
        else
        {
            mapCreator.GenerateTileMap(tileStartPos, sizeX, sizeY);
            isTileSet = true;
        }
    }



    // Update is called once per frame
    void Update()
    {
    }



    /// <summary>
    /// 해당 노드 위치에 있는 타일 반환
    /// </summary>
    /// <param name="x"> row </param>
    /// <param name="y"> column </param>
    /// <returns> Tile </returns>
    public Tile GetTile(int x, int y)
    {
        Node n = new Node(x, y);
        var tile = tilesList.Where(t => t.node == n);

        Tile returnVal = tile.SingleOrDefault(); //1개 데이터만 허용

        return returnVal; 
    }

    /// <summary>
    ///  해당 노드 위치에 있는 타일 반환
    /// </summary>
    /// <param name="node"></param>
    /// <returns>Tile</returns>
    public Tile GetTile(Node node)
    {
        Node n = node;
        var tile = tilesList.Where(t => t.node == n);

        Tile returnVal = tile.SingleOrDefault(); //1개 데이터만 허용

        return returnVal;
    }
}
