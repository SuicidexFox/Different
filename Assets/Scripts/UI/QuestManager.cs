using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;


public class QuestManager : MonoBehaviour
{
    public CinemachineVirtualCamera _questCam;
    [SerializeField] private List<Quest> _quests;

    private void Start()
    {
        foreach (Quest d in GetComponentsInChildren<Quest>())
        {
            _quests.Add(d);
        }
    }

    public void ShowQuest()
    {
        GetPrioritizedQuests().ShowQuest();
        _questCam.Priority = 11;

    }

    private Quest GetPrioritizedQuests()
    {
        Quest prioritizedQuests = _quests[0];

        foreach (Quest d in _quests)
        {
            if (prioritizedQuests.priority < d.priority)
            {
                prioritizedQuests = d;
            }
        }

        return prioritizedQuests;
    }
}