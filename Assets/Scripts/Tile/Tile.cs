using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public Node node = new Node();

    public int height;

    public bool isBlock;

    public bool isOnUnit = false;

    public bool ImpossibleUnitSetTile = false;

    Vector3 size;
    List<Color> colors = new List<Color>();
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 해당 클래스 미니언을 배치할 수 있는 타일인지 확인
    /// </summary>
    /// <returns>true일시 배치 가능</returns>
    public bool IsDeployableMinionTile()
    {
        return (!isBlock && !isOnUnit && !ImpossibleUnitSetTile);
    }

    /// <summary>
    /// 배치가능한 타일 녹색으로 표시
    /// </summary>
    /// <param name="isActive"></param>
    /// <param name="minionClass"></param>
    public void ShowDeployableTile(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 배치가능한 타일 녹색으로 표시
    /// </summary>
    /// <param name="minionClass"></param>
    /// <param name="isActive"></param>
    public void ShowOffenceModeDeployableTile(MinionClass minionClass, bool isActive)
    {

    }

    public bool onTile(Transform enemy)
    {
        if (enemy.transform.position.x > transform.position.x - size.x / 2 &&
            enemy.transform.position.x < transform.position.x + size.x / 2 &&
            enemy.transform.position.z > transform.position.z - size.z / 2 &&
            enemy.transform.position.z < transform.position.z + size.z / 2)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    public void on(bool b)
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
