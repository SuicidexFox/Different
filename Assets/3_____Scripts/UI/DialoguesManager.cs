using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;



public class DialoguesManager : MonoBehaviour
{
    public CinemachineVirtualCamera _dialogCam;
    [SerializeField] private List<Dialog> _dialogues;
    
    
    private void Start()
    {
        foreach (Dialog d in GetComponentsInChildren<Dialog>())
        {
            _dialogues.Add(d); 
        }
    }
    public void ShowDialogue()
    {
        GetPrioritizedDialogue().ShowDialogue();
        _dialogCam.Priority = 11;
    }
    private Dialog GetPrioritizedDialogue()
    {
        Dialog prioritizedDialogue = _dialogues[0];

        foreach (Dialog d in _dialogues)
        {
            if (d._needImportantItem != "") //wenn er das Item nicht hat
                if (GameManager.instance._importantItems.Contains(d._needImportantItem))
                {
                    return d;
                }
                else
                {
                    continue;
                }
        if (prioritizedDialogue.priority < d.priority)
            {
                prioritizedDialogue = d;
            }
        }

        return prioritizedDialogue;
    }
}