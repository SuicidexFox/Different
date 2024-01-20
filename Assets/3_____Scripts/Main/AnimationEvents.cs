using System.Collections;
using System.Collections.Generic;
using System.Net;
using _3_____Scripts.Main;
using FMODUnity;
using UnityEditor;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{       ///////////////////////////////////// Variablen \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private PlayerController player;
    
        ///////////////////////////////////// Player \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void PlaySound(string soundPath) { RuntimeManager.PlayOneShot(soundPath); }
    public void PlayerFootstep()
    {
        string ground = GetGround();
        string soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Tile"; 
        //Kitchen
        if (ground == "Tiles") { soundPath = "event:/SFX/Rosie/RosieFootsteps/Footstep_Tile"; }
        //Psychatrie
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
    public void StartInput() { player.ActivateInput(); GameManager.instance.fadeIn.SetActive(false);}
    public void CloseDialogUI() { GameManager.instance.AnimationCloseDialogUI(); }
    public void SelectQuestButton() { GameManager.instance.AnimationSelectButtonQuestUI(); }
    public void CloseQuestUI() { GameManager.instance.AnimationCloseQuestUI(); }
    public void SelectFadeOutButton() { GameManager.instance.AnumationSelectButtonFadeOutUI(); }
}
