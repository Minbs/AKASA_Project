using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField]
    private bool toggleMaster, toggleBGM, toggleEffects;

    public void Toggle()
    {
        if (toggleMaster) SoundManager.Instance.ToggleMuteMaster();
        if (toggleBGM) SoundManager.Instance.ToggleMuteBGM();
        if (toggleEffects) SoundManager.Instance.ToggleMuteEffects();
    }
}
