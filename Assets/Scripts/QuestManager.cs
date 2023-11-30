using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


[Serializable]
public class QuestManagerLine
{
    [TextArea(3, 8)]public string _Questtext;
}



public class QuestManager : MonoBehaviour
{
    public CinemachineVirtualCamera _questCam; 
    public void ShowQuestText() 
    { 
        GameManager.instance.ShowQuestUI(); 
        _questCam.Priority = 11;
        
    }
}