using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Debug = System.Diagnostics.Debug;

public class QuestManager : MonoBehaviour
{
    [Header("ShowQuest")]
    [TextArea(3, 8)] public string text;

    public void ShowQuestUI()
    {
        GameManager.instance.ShowQuestUI(this);
        CapsuleCollider[] capsuleColliders = GetComponentsInChildren<CapsuleCollider>();
        foreach (CapsuleCollider cc in capsuleColliders) { cc.enabled = true; } 
    }
}
