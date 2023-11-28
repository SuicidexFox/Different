using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public bool _imageRosie;
    public bool _talkbox;
    public bool _thinkbox;
    [TextArea(3, 8)]public string _text;
    [TextArea(3, 8)]public string _textNPC;
    public bool _imageTherapist;
    public bool _imageErgo;
    public bool _imageInnerChilde;

    public UnityEvent _lineEvent;
    public List<Buttons> _Buttons;
}


public class Dialog : MonoBehaviour
{
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

