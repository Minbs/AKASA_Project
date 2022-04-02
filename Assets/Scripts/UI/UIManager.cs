using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
public class UIManager : Singleton<UIManager>
{
    public List<Node> attackRangeNodes = new List<Node>();
    public List<GameObject> attackRangeTileImages = new List<GameObject>();

    public GameObject attackRangeTileImage;
    public GameObject worldCanvas;

    public GameObject settingCharacter;
    public SkeletonDataAsset skeletonDataAsset;

    public bool isSettingCharacterOn = false;

    void Start()
    {
        ObjectPool.Instance.CreatePoolObject("AttackRangeTile", attackRangeTileImage, 20, worldCanvas.transform);
    }

    void Update()
    {
        ShowSettingCharacter();
    }

   public void ShowSettingCharacter()
    {
        settingCharacter.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }


    public void RemoveAttackRangeTiles()
    {
        if (attackRangeTileImages.Count > 0)
        {

            foreach (var r in attackRangeTileImages)
            {
                ObjectPool.Instance.PushToPool("AttackRangeTile", r, worldCanvas.transform);

            }

            attackRangeTileImages.Clear();

        }
    }

    public void ShowAttackRangeTiles(bool isActive, Tile tile = null)
    {
        if (isActive)
        {
            RemoveAttackRangeTiles();

            foreach (var t in attackRangeNodes)
            {
                Tile tile1 = BoardManager.Instance.GetTile(t + tile.node);

                if (tile1 != null)
                {
                    Vector3 pos = tile1.gameObject.transform.position;

                    GameObject attackTile = ObjectPool.Instance.PopFromPool("AttackRangeTile");
                    pos.y += 0.151f;
                    attackTile.GetComponent<RectTransform>().position = pos;
                    attackTile.SetActive(true);
                    attackRangeTileImages.Add(attackTile);
                }
            }
        }
        else
        {
            RemoveAttackRangeTiles();
        
        }
    }

}
