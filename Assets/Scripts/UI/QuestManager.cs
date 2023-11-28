using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;



[Serializable]
public class Questlines 
{
    [TextArea(3, 8)]public string _Questtext;
}

public class QuestManager : MonoBehaviour
{
    public CinemachineVirtualCamera _questCam;
    public List<Questlines> _questlines;
    public Animator _animationQeustUI;
    
    
    public void ShowQuestLine()
    {
         {
             GameManager.instance.ShowQuestUI(this);
             _questCam.Priority = 11;
         }
    }
}