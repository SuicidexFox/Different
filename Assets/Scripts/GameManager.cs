using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    
    public GameObject interactUI;
    

    private void Awake()
    {
        instance = this;
    }

    public void ShowInteractUI(bool show)
    {
        interactUI.SetActive(show);
    }
}
