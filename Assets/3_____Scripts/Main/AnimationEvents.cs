using System.Collections;
using System.Collections.Generic;
using System.Net;
using FMODUnity;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private PlayerController player;
    private QuestManager questManager;
    public void PlaySound(string soundPath)
    {
        RuntimeManager.PlayOneShot(soundPath);
    }
    
    
    //Player
    public void PlayerFootstep()
    { 
        RuntimeManager.PlayOneShot("Assets/Sounds/Footstep Wood 1.wav");
    }
    public void PlayerTake() 
    { GameManager.instance.DestroyInteractable(); }
    //public void AnimationHalloEnd() { _player.AnimationHelloClose(); }
    
    
    //UI
    public void UIDialogClose()
    { GameManager.instance.AnimationEventCloseDialogUI(); }
    public void UIQuestClose() 
    { GetComponentInParent<QuestManager>().CloseQuestUI(); }
}
