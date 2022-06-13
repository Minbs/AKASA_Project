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
    /// 해당 클래스 미니언을 배치할 수 있는 타일인지 확인
    /// </summary>
    /// <returns>true일시 배치 가능</returns>
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
    /// 배치가능한 타일 표시
    /// </summary>
    /// <param name="isActive"></param>
    /// <param name="minionClass"></param>
    public void ShowDeployableTile(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


}
