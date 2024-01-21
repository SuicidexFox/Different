using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{       ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private PlayerController playerController;
    
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
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            return hit.collider.tag;
        }
        else
        {
            return "";
        }
    }
    public void PlayerTake() { GameManager.instance.DestroyInteractable(); }
    

        ///////////////////////////////////// Canvas \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void StartInput() { playerController.ActivateInput(); GameManager.instance.fade.SetActive(false); }
    public void CloseDialogUI() { GameManager.instance.AnimationCloseDialogUI(); }
    public void SelectQuestButton() { GameManager.instance.AnimationSelectButtonQuestUI(); }
    public void CloseQuestUI() { GameManager.instance.AnimationCloseQuestUI(); }
    public void SelectFadeOutButton() { GameManager.instance.AnumationSelectButtonFadeOutUI(); }
    public void FadeOutShort() { GameManager.instance.Scenes();}
}
