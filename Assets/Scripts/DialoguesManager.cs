using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguesManager : MonoBehaviour
{
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
    }
  
    private Dialog GetPrioritizedDialogue()
    {
        Dialog prioritizedDialogue = _dialogues[0];

        foreach (Dialog d in _dialogues)
        {
            if (prioritizedDialogue.priority < d.priority)
            {
                prioritizedDialogue = d;
            }
        }

        return prioritizedDialogue;
    }
}