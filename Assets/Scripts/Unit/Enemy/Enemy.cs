using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Enemy : Unit
{
    // Start is called before the first frame update
    List<Tile> moveTiles = new List<Tile>();
    public Image healthBar;



    protected override void Start()
    {
        base.Start();

        moveTiles = BoardManager.Instance.FinalList.ToList();

        poolItemName = "Enemy1";
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)currentHp / maxHp;

        if (BoardManager.Instance.end == true && moveTiles.Count != 0)
        {
            Move();
        }
        else if (BoardManager.Instance.end == true && moveTiles.Count == 0)
        {
            GameManager.Instance.enemiesList.Remove(gameObject);
            ObjectPool.Instance.PushToPool(poolItemName, gameObject);
        }
    }

    void Move()
    {
        Vector3 des = moveTiles[0].transform.position;
        des.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, des, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, des) < 0.01f)
        {
            transform.position = des;
            moveTiles.RemoveAt(0);
        }
    }
}
