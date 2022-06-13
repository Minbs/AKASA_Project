using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMouseOver : MonoBehaviour
{
    public GameObject SelectButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        SelectButton.SetActive(true);
        this.gameObject.SetActive(false);

    }
}
