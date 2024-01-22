using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("ShowQuest")]
    public CinemachineVirtualCamera questCam;
    public GameObject currentLetter;
    [TextArea(3, 8)] public string text;
    public List<Interactable> questObjects;
    public List<CapsuleCollider> capsuleCollider;

    private void Start()
    {
        foreach (Interactable im in GetComponentsInChildren<Interactable>())
        {
            questObjects.Add(im);
        }
        foreach (CapsuleCollider cc in GetComponentsInChildren<CapsuleCollider>())
        {
            capsuleCollider.Add(cc);
            enabled = true;
        }
    }
    public void ShowQuestUI()
    {
        if (questCam != null) { questCam.Priority = 11; }
        GameManager.instance.ShowQuestUI(this);
    }
    public void DestroyInteractable()
    {
        GetComponentsInChildren<Interactable>(gameObject.CompareTag("Dishes"));
        {
            gameObject.SetActive(false);
        }
    }
}
