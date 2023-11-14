using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class DialoguesLines
{
    public GameObject _chatbox;
    public GameObject _charakter;
    [TextArea(4, 10)]public string _text;
}


public class Dialog : MonoBehaviour
{
    public int priority = -1;
    public List<DialoguesLines> dialoguesLines;
    public UnityEvent dialogEnd;

    public void ShowDialogue()
    {
        if (dialoguesLines.Count == 0 || GameManager.instance._inDialogue) { return; }
        GameManager.instance.ShowDialog(this);
    }

    public void SetPriority(int newPriority)
    {
        priority = newPriority;
    }
}