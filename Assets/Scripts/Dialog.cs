using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class DialoguesLines
{
    [TextArea(3, 10)]public string text;
}


public class Dialog : MonoBehaviour
{
    public int priority = -1;
    public List<DialoguesLines> dialoguesLines;
    public UnityEvent dialogEnd;
    public UnityEvent OpenDoor;

    public void ShowDialogue()
    {
        if (dialoguesLines.Count == 0 || GameManager.instance._inDialogue) { return; }
        
        GameManager.instance.ShowDialogue(this);
        
    }

    public void SetPriority(int newPriority)
    {
        priority = newPriority;
    }
}