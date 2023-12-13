using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private PlayerController _player;
    public void PlaySound(string soundPath)
    {
        RuntimeManager.PlayOneShot(soundPath);
    }
    
    
    //Player
    public void Footstep()
    {
            RuntimeManager.PlayOneShot("Assets/Sounds/Footstep Wood 1.wav");
    }
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

    public void CloseEndUI()
    {
        GameManager.instance.CloseEndeUI();
    }
}