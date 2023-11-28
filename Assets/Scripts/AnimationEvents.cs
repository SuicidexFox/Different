using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void PlaySound(string soundPath)
    {
        RuntimeManager.PlayOneShot(soundPath);
    }
    public void Footstep()
    {
        RuntimeManager.PlayOneShot("event:/SFX/Footstep_Stone");
    }

    public void TriggerQuestClose()
    {
        GameManager.instance.CloseQuestUI();
    }

    public void AnimationCollect()
    {
        GameManager.instance.TakeCollect();
    }
    
    
    
    //UI Animator
    public void CloseDialogUI()
    {
        GameManager.instance.AnimationEventCloseDialogUI();
    }
}
