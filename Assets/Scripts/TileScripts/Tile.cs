using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public Point pos  = new Point();

    public int height;

    public bool isEmpty;

    //타일 타입 추가


    //길찾기 알고리즘
    public int G; 
    public int H;
    public Tile parentTile;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTileInfo(string[] s)
    {
        pos.x = int.Parse(s[0]);
        pos.y = int.Parse(s[1]);
        height = int.Parse(s[2]);
        isEmpty = (s[3] == "O") ? true : false; 
    }
}
