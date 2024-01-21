using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    [Header("ShowQuest")]
    public CinemachineVirtualCamera questCam;
    public GameObject currentLetter;
    [TextArea(3, 8)] public string text;
    public List<InteractableManager> questObjects;
    public List<CapsuleCollider> capsuleCollider;

    private void Start()
    {
        foreach (InteractableManager im in GetComponentsInChildren<InteractableManager>())
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
        GetComponentsInChildren<InteractableManager>(gameObject.CompareTag("Dishes"));
        {
            gameObject.SetActive(false);
        }
    }
}