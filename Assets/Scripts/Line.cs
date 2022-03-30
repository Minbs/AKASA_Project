using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer line;
    public Transform pos1;
    public Transform pos2;

    float timer;
    float waitingTime;

    List<Tile> lineTile = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
        timer = 0.0f;
        waitingTime = 0.5f;
        Debug.Log(BoardManager.Instance.FinalList);
    }

    // Update is called once per frame
    void Update()
    {
        if (BoardManager.Instance.end == true && lineTile.Count == 0)
        {
            lineTile = BoardManager.Instance.FinalList;
        }
        else if (BoardManager.Instance.end == true && lineTile.Count != 0)
        {
            //Invoke("CreateLine", 2.0f);
            CreateLine();
        }
    }

    void CreateLine()
    {
        Vector3 vec = lineTile[0].transform.position;
        vec.y = 0.2f;

        line.SetPosition(0, vec);
        line.SetPosition(1, lineTile[1].transform.position);
        lineTile.RemoveAt(0);

        foreach (var t in BoardManager.Instance.tilesList)
        {
            timer += Time.deltaTime;

            if (timer > waitingTime)
            {
                if (t.height == 0)
                    t.onTile(transform);
                timer = 0;
            }
        }
    }
}
