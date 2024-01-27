using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AnimationEvent : MonoBehaviour
{   
    ///////////////////////////////////// UI_UX \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    ///////////////////////////////////////// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    ///////////////////////////////////// DialogUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void CloseDialogUI() { GameManager.instance.AnimationEventCloseDialogUI(); }
    
    ///////////////////////////////////// QuestUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void SelectQuestButton() { GameManager.instance.AnimationSelectButtonQuestUI(); }
    public void CloseQuestUI() { { GameManager.instance.AnimationEventCloseQuestUI(); } }
    
    ///////////////////////////////////// QuestLog \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void PlayOneShotQuestTab() {RuntimeManager.PlayOneShot("event:/SFX/UI_UX/QuestUI/QuestUIClose");}
    public void QuestTabLetterDestroy() { Destroy(gameObject); }
    
    ///////////////////////////////////////// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    ///////////////////////////////////// Player \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void PlaySound(string soundPath) { RuntimeManager.PlayOneShot(soundPath); }
    public void PlayerFootstep()
    {
        string ground = GetGround();
        string soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Tile"; 
        //Kitchen
        if (ground == "Tiles") { soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Tile"; }
        //Psychiatry
        if (ground == "Wood") { soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Wood"; }
        if (ground == "Carpet") { soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Capet"; }
        //Save Place
        if (ground == "Stone") { soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Stone"; }
        if (ground == "Grass") { soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Grass"; }
        RuntimeManager.PlayOneShot(soundPath);
    }
    private string GetGround()
    {
        float rayDistance = 1.0f;
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        RaycastHit hit;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        
        if (Physics.Raycast(ray, out hit, rayDistance)) { return hit.collider.tag; }
        else { return ""; }
    }
    public void PlayerTake() 
    { 
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Collect/Take 3");
        GameManager.instance.DestroyInteractable();
    }
    public void PlayerInput() { GameManager.instance.playerController.ActivateInput(); }
    public void PlayerPlaySitUp() { RuntimeManager.PlayOneShot("event:/SFX/Rosie/RosieFootsteps/Footstep_Capet"); }
}
