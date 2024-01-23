using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvent : MonoBehaviour
{   
    ///////////////////////////////////// UI_UX \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void CloseDialogUI() { GameManager.instance.AnimationEventCloseDialogUI(); }
    
    
    
    public void SelectQuestButton() { GameManager.instance.AnimationSelectButtonQuestUI(); }

    public void CloseQuestUI() { GameManager.instance.AnimationEventCloseQuestUI(); }
    
    
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
    public void PlayerTake() { GameManager.instance.DestroyInteractable(); }
    public void PlayerInput() { GameManager.instance.playerController.ActivateInput(); }
    public void PlayerPlaySitUp() { RuntimeManager.PlayOneShot("event:/SFX/Rosie/RosieFootsteps/Footstep_Capet"); }
    
    
    ///////////////////////////////////////// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    /////////////////////////////////////// NPC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void Scrible() { RuntimeManager.PlayOneShot("event:/SFX/Therapist/Pencil"); }
    
}
