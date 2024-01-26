using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class ButtonLines
{
    public string text;
    public UnityEvent buttonEvent;
}

[Serializable]
public class DialoguesLines
{
    public EventReference sound;
    public Sprite character;
    public Sprite textBox;
    [TextArea(3, 8)]public string text;
    [TextArea(3, 8)]public string textNpc;

    public UnityEvent lineEvent;
    public List<ButtonLines> buttonLinesList;
}


public class Dialog : MonoBehaviour
{
    public string needImportantItem;
    public int priority;
    public List<DialoguesLines> DialoguesLinesList;
    public UnityEvent dialogEnd;


    public void ShowDialogue()
    {
        if (DialoguesLinesList.Count == 0 || GameManager.instance.inUI)
        {
            return;
        }

        GameManager.instance.ShowDialogUI(this);
    }

    public void SetPriority(int newPriority) { priority = newPriority; }
}
