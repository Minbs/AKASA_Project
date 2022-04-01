using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Line : MonoBehaviour
{
    public LineRenderer line;

    float time;
    float waitingTime;

    List<Tile> lineTile = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 3;
        time = 0;
        waitingTime = 0.175f;
    }

    // Update is called once per frame
    void Update()
    {
        if (BoardManager.Instance.end == true && lineTile.Count == 0)
        {
            lineTile = BoardManager.Instance.FinalList.ToList();
        }
        else if (BoardManager.Instance.end == true && lineTile.Count != 0)
        {
            //Invoke("CreateLine", 1.0f);
            CreateLine();
        }
    }

    void CreateLine()
    {
        if (lineTile.Count <= 0)
        {
            return;
        }

        time += Time.deltaTime;

        if (time >= waitingTime)
        {
            if (lineTile.Count > 2)
            {
                Vector3 vec = lineTile[0].transform.position;
                vec.y = 0.2f;
                Vector3 vec1 = lineTile[1].transform.position;
                vec1.y = 0.2f;
                Vector3 vec2 = lineTile[2].transform.position;
                vec2.y = 0.2f;

                line.SetPosition(0, vec);
                line.SetPosition(1, vec1);
                line.SetPosition(2, vec2);

                //Vector3 lineVec = Vector3.Lerp(vec, vec1, waitingTime * Time.deltaTime);
                //Vector3 lineVec1 = Vector3.Lerp(vec1, vec1, waitingTime * Time.deltaTime);

                //line.SetPosition(0, lineVec);
                //line.SetPosition(1, lineVec1);
            }
            lineTile.RemoveAt(0);

            time = 0;
        }
    }
}
