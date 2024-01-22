using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;


public class QuestManager : MonoBehaviour
{
    [Header("ShowQuest")]
    public CinemachineVirtualCamera _questCam;
    public GameObject _currentLetter;
    [TextArea(3, 8)] public string _text;
    public List<InteractableManager> _questObejcts;

    private void Start()
    {
        foreach (InteractableManager im in GetComponentsInChildren<InteractableManager>())
        {
            _questObejcts.Add(im);
        }
    }
    public void ShowQuestUI()
    {
        _questCam.Priority = 11;
        GameManager.instance.ShowQuestUI(this);
    }
}