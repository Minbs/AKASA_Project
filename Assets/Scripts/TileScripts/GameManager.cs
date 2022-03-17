using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static List<Tile> tilesList = new List<Tile>();

    private int sizeX = 7;
    private int sizeY = 6;

    List<Tile> OpenList;
    List<Tile> CloseList;
    public List<Tile> FinalList;

    Tile currentTile;
    Tile startTile;
    Tile endTile;

    public bool isTileSet = false;

    public MapCreator mapCreator;

    public bool end = false;

    public static GameManager instance;




    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        OpenList = new List<Tile>();
        CloseList = new List<Tile>();
        FinalList = new List<Tile>();

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
           endTile = GetTile(6, 4);


           PathFinding();
            end = true;
       }
    }

    void PathFinding()
    {
        OpenList.Add(startTile);
  

        while (OpenList.Count > 0) //OpenList에서 F값을 비교 후 currentTile 변경
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

            AddOpenList(currentTile.pos.x, currentTile.pos.y + 1);
            AddOpenList(currentTile.pos.x + 1, currentTile.pos.y);
            AddOpenList(currentTile.pos.x, currentTile.pos.y - 1);
            AddOpenList(currentTile.pos.x - 1, currentTile.pos.y);
        }
    }

    void AddOpenList(int x, int y)
    {
        Tile neighborTile = GetTile(x, y); 
        if(x >= 0 && x < sizeX && y >= 0 && y < sizeY && neighborTile.isEmpty && neighborTile.height == 0 && !CloseList.Contains(neighborTile)) //타일 범위 안, 비어있음, 높이가 0, ClosedList에 없을 때
        {


            int Cost = currentTile.G + (currentTile.pos.x - x == 0 || currentTile.pos.y - y == 0 ? 10 : 14); //직선 거리 10, 대각선 14

            if(Cost < neighborTile.G || !OpenList.Contains(neighborTile))
            {
                neighborTile.G = Cost;
                neighborTile.H = (Mathf.Abs(neighborTile.pos.x - endTile.pos.x) + Mathf.Abs(neighborTile.pos.y - endTile.pos.y)) * 10;
                neighborTile.parentTile = currentTile;

                OpenList.Add(neighborTile);
            }
        }
    }

    Tile GetTile(int x, int y)
    {
        Point p = new Point(x, y);
        var tile = tilesList.Where(t => t.pos == p);

        Tile t = tile.SingleOrDefault(); //1개 데이터만 허용
        return t; 
    }
}
