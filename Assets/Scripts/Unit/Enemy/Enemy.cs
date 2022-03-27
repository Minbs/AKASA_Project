using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    List<Tile> moveTiles = new List<Tile>();
    public int mode = 0;

    public string poolItemName = "Enemy1";

    void Start()
    {
        moveTiles = BoardManager.Instance.FinalList.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if(BoardManager.Instance.end == true && moveTiles.Count != 0)
        {
            Move();
        }
        else if (BoardManager.Instance.end == true && moveTiles.Count == 0)
        {
            ObjectPool.Instance.PushToPool(poolItemName, gameObject);
        }
    }

    void Move()
    {
        Vector3 des = moveTiles[0].transform.position;
        des.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, des, 1 * Time.deltaTime);

        if(Vector3.Distance(transform.position, des) < 0.01f)
        {
            transform.position = des;
            moveTiles.RemoveAt(0);
        }
    }
}
