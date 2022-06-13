using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// �ش� Ŭ���� �̴Ͼ��� ��ġ�� �� �ִ� Ÿ������ Ȯ��
    /// </summary>
    /// <returns>true�Ͻ� ��ġ ����</returns>
    public bool IsDeployableMinionTile()
    {
        return (!isBlock && !isOnUnit && !ImpossibleUnitSetTile);
    }

    /// <summary>
    /// ��ġ������ Ÿ�� ������� ǥ��
    /// </summary>
    /// <param name="isActive"></param>
    /// <param name="minionClass"></param>
    public void ShowDeployableTile(bool isActive)
    {


        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// ��ġ������ Ÿ�� ������� ǥ��
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

    public void SetTileClassImage(MinionClass minionClass)
    {
        Sprite tileSprite;

        switch (minionClass)
        {
            case MinionClass.Buster:
                tileSprite = BattleUIManager.Instance.busterTileSprite;
                break;
            case MinionClass.Guardian:
                tileSprite = BattleUIManager.Instance.guardianTileSprite;
                break;
            case MinionClass.Chaser:
                tileSprite = BattleUIManager.Instance.chaserTileSprite;
                break;
            case MinionClass.Rescue:
                tileSprite = BattleUIManager.Instance.rescueTileSprite;
                break;
            default:
                tileSprite = BattleUIManager.Instance.DeployableTileSprite;
                break;
        }

        GetComponent<Image>().sprite = tileSprite;
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
