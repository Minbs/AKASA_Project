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
        line.positionCount = 2;
        time = 0;
        waitingTime = 0.5f;
        Debug.Log(BoardManager.Instance.FinalList);
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

        Vector3 vec = lineTile[0].transform.position;
        vec.y = 0.2f;

        time += Time.deltaTime;

        if (time >= waitingTime)
        {
            line.SetPosition(0, vec);

            if (lineTile.Count > 1)
            {
                line.SetPosition(1, lineTile[1].transform.position);
            }
            lineTile.RemoveAt(0);

            time = 0;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
