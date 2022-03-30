using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField]
    public List<Tile> tilesList = new List<Tile>();

    public int sizeX;
    public int sizeY;

    List<Tile> OpenList;
    List<Tile> CloseList;
    public List<Tile> FinalList;

    Tile currentTile;
    Tile startTile;
    Tile endTile;

    public bool isTileSet = false;

    public bool end = false;

    private MapCreator mapCreator;

    bool tileSetMode = false;


    // Start is called before the first frame update
    void Awake()
    {

        OpenList = new List<Tile>();
        CloseList = new List<Tile>();
        FinalList = new List<Tile>();

        mapCreator = new MapCreator();

        if (tilesList == null)
            Debug.Log("n");
        else
        {
            mapCreator.Load();
            isTileSet = true;
        }

    }



    // Update is called once per frame
    void Update()
    {
       if(FinalList.Count <= 0 && isTileSet)
       {
           startTile = GetTile(0, 1);
           endTile = GetTile(17, 4);


           PathFinding();
            end = true;
       }
    }

    void PathFinding()
    {
        OpenList.Add(startTile);
  

        while (OpenList.Count > 0) //OpenList���� F���� �� �� currentTile ����
        {
            currentTile = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                int F1 = OpenList[0].G + OpenList[0].H;
                int F2 = currentTile.G + currentTile.H;

                if (F1 <= F2 && OpenList[i].H < currentTile.H)
                    currentTile = OpenList[i];
            }

            OpenList.Remove(currentTile);
            CloseList.Add(currentTile);

            if(currentTile == endTile)
            {
                Tile targetCurTile = endTile;
                while(targetCurTile != startTile)
                {
                    FinalList.Add(targetCurTile);
                    targetCurTile = targetCurTile.parentTile;
                }
                FinalList.Add(startTile);
                FinalList.Reverse();
             
            }

            AddOpenList(currentTile.node.row, currentTile.node.column + 1);
            AddOpenList(currentTile.node.row + 1, currentTile.node.column);
            AddOpenList(currentTile.node.row, currentTile.node.column - 1);
            AddOpenList(currentTile.node.row - 1, currentTile.node.column);
        }
    }

    void AddOpenList(int x, int y)
    {
        Tile neighborTile = GetTile(x, y); 
        if(x >= 0 && x < sizeX && y >= 0 && y < sizeY && !neighborTile.isBlock && neighborTile.height == 0 && !CloseList.Contains(neighborTile)) //Ÿ�� ���� ��, �������� ����, ���̰� 0, ClosedList�� ���� ��
        {


            int Cost = currentTile.G + (currentTile.node.row - x == 0 || currentTile.node.column - y == 0 ? 10 : 14); //���� �Ÿ� 10, �밢�� 14

            if(Cost < neighborTile.G || !OpenList.Contains(neighborTile))
            {
                neighborTile.G = Cost;
                neighborTile.H = (Mathf.Abs(neighborTile.node.row - endTile.node.row) + Mathf.Abs(neighborTile.node.column - endTile.node.column)) * 10;
                neighborTile.parentTile = currentTile;

                OpenList.Add(neighborTile);
            }
        }
    }

    Tile GetTile(int x, int y)
    {
        Node n = new Node(x, y);
        var tile = tilesList.Where(t => t.node == n);

        Tile returnVal = tile.SingleOrDefault(); //1�� �����͸� ���


        return returnVal; 
    }

    public void CanSetTile()
    {
        if (tileSetMode)
            tileSetMode = false;
        else
            tileSetMode = true;


        foreach (var tile in tilesList)
        {
            tile.canUnitSetTile(tileSetMode);
        }
    }
}
