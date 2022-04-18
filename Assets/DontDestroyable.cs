using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyable : Singleton<DontDestroyable>
{
    public List<AudioClip> AudioList;
    private void Awake()
    {
        if (Instance != null)
        {
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (DontDestroyable.Instance == this)
        {
            AudioPlay(0);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            this.gameObject.GetComponent<AudioSource>().Stop();
            //Instance.SetEdit();
            Destroy(this.gameObject);
        }
    }

    public void AudioPlay(int recordnum)
    {
        this.gameObject.GetComponent<AudioSource>().clip = AudioList[recordnum];
        this.gameObject.GetComponent<AudioSource>().Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
