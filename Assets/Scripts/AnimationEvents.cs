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
    
    
    //Player
    public void Footstep() { RuntimeManager.PlayOneShot("event:/SFX/Footstep_Stone"); }
    public void Take() { GameManager.instance.DestroyCollect();  }
    
    
    
    //UI Animator
    public void CloseDialogUI()
    {
        GameManager.instance.AnimationEventCloseDialogUI();
    } 
    public void TriggerQuestClose() 
    { 
        GameManager.instance.CloseQuestUI();
    }
}
