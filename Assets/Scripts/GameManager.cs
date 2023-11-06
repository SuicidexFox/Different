using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    
    [Header("Interact")]
    [SerializeField] private GameObject _InteractUI;
    [Header("Dialogue")]
    [SerializeField] private GameObject _DialogUI;
    [SerializeField] private TextMeshProUGUI _Text;
    [SerializeField] private Button _countiniueButton;

    private Dialog _currentLines;
    private int _currentLineIndex;
    public bool _inDialogue = false;
    

    private void Awake() //nur beim ersten Start der gesamten Instanz
    {
        instance = this;
    }
    private void Start()
    {
       _InteractUI.SetActive(false);
       _DialogUI.SetActive(false);
   
    }

    
    //Anzeige der Tasten und Hinweise vllt. Tutorial
    public void ShowIneractUI(bool show)
    {
        _InteractUI.SetActive(show);
    }
    
    
    //Dialog
    public void ShowDialogUI(bool show)
    { 
        _DialogUI.SetActive(show);
        
    }
    public void ShowDialogue(Dialog dialog)
    {
        _inDialogue = true;
        _currentLines = dialog;
        _currentLineIndex = 0;
        ShowIneractUI(false);

        _Text.SetText("");
        
        Cursor.lockState = CursorLockMode.Confined;
        _DialogUI.SetActive(true);
        ShowCurrentLine();
    }
    private void ShowCurrentLine()
    {
        DialoguesLines dialogueLines = _currentLines.dialoguesLines[_currentLineIndex];
        if (dialogueLines == null) { return; } 
        
        
        _Text.SetText(dialogueLines.text);
    }
    public void NextLine()
    {
        if (!_inDialogue) { return; }
        
        _currentLineIndex++; //einfach immer eins weiter zählen

        if (_currentLines.dialoguesLines.Count == _currentLineIndex)
        {
            CloseDialog();
            return;
        }
        
        ShowCurrentLine();
    }
    public void CloseDialog()
    {
      
        _DialogUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        _inDialogue = false;
    }
    
    
    
    
    
}
