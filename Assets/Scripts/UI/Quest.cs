using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.VisualScripting;

[Serializable]
public class QuestsLines
{
    [TextArea(3, 8)]public string _Questtext;
}




public class Quest : MonoBehaviour
{
    public int priority = -1;
    public List<QuestsLines> questsLines;

    public void ShowQuest()
    {
        if (questsLines.Count == 0) { return; }
        GameManager.instance.ShowQuest(this);
    }

    public void SetPriority(int newPriority)
    {
        priority = newPriority;
    }
}
