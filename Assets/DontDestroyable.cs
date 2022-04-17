using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyable : Singleton<DontDestroyable>
{
    private void Awake()
    {
        if (Instance != null)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (DontDestroyable.Instance == this)
        {
            ;
        }
        else
        {
            //Instance.SetEdit();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
