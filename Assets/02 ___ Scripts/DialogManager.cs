using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public CinemachineVirtualCamera dialogCam;
    [SerializeField] private List<Dialog> dialogues;
    
    private void Start()
    {
        foreach (Dialog d in GetComponentsInChildren<Dialog>())
        {
            dialogues.Add(d); 
        }
    }

    public void ShowDialogue()
    {
        GetPrioritizedDialogue().ShowDialogue();
        if (dialogCam == null) { return; }
        dialogCam.Priority = 11;
    }
    private Dialog GetPrioritizedDialogue()
    {
        Dialog prioritizedDialogue = dialogues[0];

        foreach (Dialog d in dialogues)
        {
            if (d.needImportantItem != "")
                if (GameManager.instance.importantItems.Contains(d.needImportantItem)) { return d; }
                else { continue; } 
            if (prioritizedDialogue.priority < d.priority) { prioritizedDialogue = d; } 
        }
        return prioritizedDialogue;
    }
}
