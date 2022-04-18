using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{   
    [SerializeField] bool b_Looked;
    [SerializeField] bool b_Selected;
    [SerializeField] List<Sprite> BtnState;
    [SerializeField] GameObject TargetMark;
    [SerializeField] string StageName;
    [SerializeField] int BestLevel;
    

    public int pro_BestLv
    {
        get { return BestLevel; }
        set
        {
            BestLevel = value;
        }
    }
    public string pro_stagename
    {
        get
        {
            return StageName;
        }
        set
        {
            StageName = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (b_Looked)
        {
            this.gameObject.GetComponent<Image>().sprite = BtnState[2];
            gameObject.GetComponent<Button>().enabled = false;
        }
        TargetMark.SetActive(false);
    }

    public void Selected()
    {
        if(!b_Looked)
        {
            if(!b_Selected)
            {
                b_Selected = true;
                gameObject.GetComponent<Image>().sprite = BtnState[1];
                TargetMark.SetActive(true);
            }
            else
            {
                b_Selected = false;
                gameObject.GetComponent<Image>().sprite = BtnState[0];
                TargetMark.SetActive(false);

            }
        }
    }
}
