using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public Node node  = new Node();

    public int height;

    public bool isBlock;

    //타일 타입 추가


    //길찾기 알고리즘
    
    public int G { get; set; }
    public int H { get; set; }
    public Tile parentTile { get; set; }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTileInfo(string[] s)
    {
        node.row = int.Parse(s[1]);
        node.column = int.Parse(s[2]);
        height = int.Parse(s[3]);
        isBlock = (s[4] == "BLOCK") ? true : false; 
    }
}
