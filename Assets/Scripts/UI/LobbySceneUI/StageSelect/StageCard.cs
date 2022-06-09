using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageCard : MonoBehaviour
{

    [Header("Animator")]
    public Animator CardAnime;

    [SerializeField]
    [Header("FrontUI")]
    public GameObject FrontCard;
    public Image StageImage;
    public TextMeshProUGUI StageNumberText;
    public Sprite ShowFrontCard;
    public TextMeshProUGUI StageTextFront;

    [Header("FrontHideUI")]
    public GameObject LockSprite;
    public Sprite HideFrontCard;

    [Header("BackUI")]
    public GameObject BackCard;
    public TextMeshProUGUI StageTextBack;
    public TextMeshProUGUI EnemyInfoText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI CompensationText;

    [Header("Info")]
    [SerializeField]
    private bool LockedStageCard;   // 현재 잠금상태인지?
    [SerializeField]
    private string StageNumber;
    [SerializeField]
    private bool m_bfront;
    [SerializeField]
    private string StageName;
    // 적 정보
    [SerializeField] 
    private string enemyInfo;
    [SerializeField] 
    private float m_fLevel;
    [SerializeField]
    private string Compensation;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Setting();
        }
        catch(Exception e )
        {
            Debug.Log(e.Message);
        }

    }

    public void FrontSet()
    {
        if (StageTextFront != null)
        {
            StageTextFront.text = StageName;
            StageTextBack.text = StageName;
        }
        else
        {
            throw new Exception("Haven't StageTextFront");
        }
        if (LockedStageCard)
        {
            LockedStageCard = true;
            FrontCard.SetActive(true);
            BackCard.SetActive(false);
            LockSprite.SetActive(true);
            FrontCard.GetComponent<Button>().enabled = false;
            StageImage.sprite = HideFrontCard;
        }
        else
        {
            LockedStageCard = false;
            FrontCard.SetActive(true);
            BackCard.SetActive(false);
            LockSprite.SetActive(false);
            FrontCard.GetComponent<Button>().enabled = true;
            StageImage.sprite = ShowFrontCard;
        }
        if(StageNumberText != null)
        {
            StageNumberText.text = StageNumber;
        }
       
    }
    public void BackSet()
    {
        if (StageTextBack != null)
        {
            StageTextBack.text = StageName;
        }
        else
        {
            throw new Exception("Haven't StageTextBack");
        }
        if (EnemyInfoText != null)
        {
            EnemyInfoText.text = enemyInfo;
        }
        else
        {
            throw new Exception("Haven't EnemyInfoText");
        }
        if (LevelText != null)
        {
            LevelText.text = m_fLevel.ToString();
        }
        else
        {
            throw new Exception("Haven't LevelText");
        }
        if (CompensationText != null)
        {
            CompensationText.text = Compensation;
        }
        else
        {
            throw new Exception("Haven't CompensationText");
        }

    }

    public void Setting()
    {
        m_bfront = true;
        if (FrontCard != null)
        {
            FrontSet();
        }
        else
        {
            throw new Exception("Haven't FrontCard");
        }
        if(BackCard != null)
        {
            BackSet();
        }
        else
        {
            throw new Exception("Haven't BackCard");
        }
        gameObject.GetComponent<Button>().onClick.AddListener(() => CardChange());

    }

    public void CardChange()
    {
        if(!m_bfront)
        {
            FrontCard.SetActive(true);
            BackCard.SetActive(false);
            m_bfront = true;
            Debug.Log($"m_bfront: false -> true");
        }
        else if (m_bfront)
        {
            FrontCard.SetActive(false);
            BackCard.SetActive(true);
            m_bfront = false;
            Debug.Log($"m_bfront: true -> false");
        }
    }

    public void OpenStage()
    {
        LockedStageCard = false;
        FrontCard.SetActive(true);
        BackCard.SetActive(false);
        LockSprite.SetActive(false);
        FrontCard.GetComponent<Button>().enabled = true;
        StageImage.sprite = ShowFrontCard;
    }

    IEnumerator changed()
    {
        yield return new WaitForSeconds(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
