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

    private void Start()
    {
        foreach (Interactable im in GetComponentsInChildren<Interactable>()) { questObjects.Add(im); }

        GameObject[] objectsToActivate = GameObject.FindGameObjectsWithTag("Active");
        foreach (GameObject obj in objectsToActivate) { obj.SetActive(true); }
        CapsuleCollider[] capsuleColliders = GetComponentsInChildren<CapsuleCollider>();
        foreach (CapsuleCollider cc in capsuleColliders) { cc.enabled = true; } 
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
