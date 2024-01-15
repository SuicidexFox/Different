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
            Debug.Log("no Ground");
            return "";
        }
    }
    public void PlayerTake() 
    { GameManager.instance.DestroyInteractable(); }
    
    
    
    
    //UI
    public void UIDialogClose()
    { GameManager.instance.AnimationEventCloseDialogUI(); }
    public void UIQuestClose() 
    { GetComponentInParent<QuestManager>().CloseQuestUI(); }
}
