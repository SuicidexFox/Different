using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Buttons
{
    public string _text;
    public UnityEvent _buttonEvent;
}

[Serializable]
public class DialoguesLines
{
    public EventReference _sound;
    public Sprite _character;
    public Sprite _textbox;
    [TextArea(3, 8)]public string _text;
    [TextArea(3, 8)]public string _textNPC;

    public UnityEvent _lineEvent;
    public List<Buttons> _Buttons;
}


public class Dialog : MonoBehaviour
{
    public string _needImportantItem;
    public int priority = -1;
    public List<DialoguesLines> dialoguesLines;
    public UnityEvent dialogEnd;
    

    public void ShowDialogue()
    {
        if (dialoguesLines.Count == 0 || GameManager.instance._inUI) { return; }
        GameManager.instance.ShowDialogUI(this);
    }
    public void SetPriority(int newPriority)
    {
        priority = newPriority;
    }
}