using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Line : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    float time = 0, waitingTime = 0.175f;

    List<Tile> lineTile = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BoardManager.Instance.end == true && lineTile.Count == 0)
        {
            //길 표시선의 개수
            line.positionCount = 3;
            lineTile = BoardManager.Instance.FinalList.ToList();
        }
        else if (BoardManager.Instance.end == true && lineTile.Count != 0)
        {
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
            }
            else
            {
                if (GameManager.Instance.waitTimer <= 0 && GameManager.Instance.state == State.BATTLE)
                {
                    line.positionCount = 0;
                    return;
                }
            }
            lineTile.RemoveAt(0);

            time = 0;
        }
    }
}
