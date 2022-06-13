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

    public void SetClassTileImage(MinionClass minionClass)
    {
        Sprite tileSprite;

        switch(minionClass)
        {
            case MinionClass.Buster:
                tileSprite = BattleUIManager.Instance.BusterTileSprite;
                break;
            case MinionClass.Chaser:
                tileSprite = BattleUIManager.Instance.ChaserTileSprite;
                break;
            case MinionClass.Guardian:
                tileSprite = BattleUIManager.Instance.GuardianTileSprite;
                break;
            case MinionClass.Rescue:
                tileSprite = BattleUIManager.Instance.RescueTileSprite;
                break;
            default:
                tileSprite = null;
                break;
        }

        GetComponent<Image>().sprite = tileSprite;
    }

    /// <summary>
    /// ��ġ������ Ÿ�� ǥ��
    /// </summary>
    /// <param name="isActive"></param>
    /// <param name="minionClass"></param>
    public void ShowDeployableTile(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


}
