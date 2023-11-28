using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


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
            if (d.dialoguesLines != "") //wenn er das Item hat
                if (GameManager.instance._collectable.Contains(d.dialoguesLines))
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