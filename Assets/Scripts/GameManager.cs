using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    public PlayerController _player;
    
    [Header("Interact")]
    [SerializeField] private GameObject _InteractUI;
    public float _takeCollect = 0f;
    [Header("Dialouge")]
    [SerializeField] private GameObject _DialogUI;
    [SerializeField] private GameObject _ImageChatbox;
    [SerializeField] private TextMeshProUGUI _TextDialog;
    [SerializeField] private GameObject _ImageCharacter;
    [SerializeField] private Button _countiniueButton;
    public bool _inDialogue = false;
    
    private Dialog _currentLines;
    private int _currentLineIndex;
    private Animator _animator;
    
    
    
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
    public void ShowUI(bool show)
    {
        if (_currentLines == null) { return; }
        _DialogUI.SetActive(true);
    }

    public void ShowDialog(Dialog dialog)
    {
        _player._playerInput.SwitchCurrentActionMap("UI");
        
        _inDialogue = true;
        _currentLines = dialog;
        _currentLineIndex = 0;
        ShowIneractUI(false);
        _TextDialog.SetText("");
        
        Cursor.lockState = CursorLockMode.Confined;
        ShowCurrentLine();
    }
    private void ShowCurrentLine()
    {
        DialoguesLines dialogueLines = _currentLines.dialoguesLines[_currentLineIndex];
        if (dialogueLines == null) { return; }
        _TextDialog.SetText(dialogueLines._text);
        _ImageChatbox.Serialize(dialogueLines._chatbox);
        _ImageCharacter.Serialize(dialogueLines._charakter);

    }
    public void NextLine()
    {
        if (!_inDialogue) { return; }
        _currentLineIndex++; //einfach immer eins weiter zählen

        if (_currentLines.dialoguesLines.Count == _currentLineIndex) { CloseDialog(); return; }
        ShowCurrentLine();
    }
    public void CloseDialog()
    {
        _currentLines.dialogEnd.Invoke();
        _currentLines.GetComponentInParent<DialoguesManager>()._dialogCam.Priority = 0;
        
        _DialogUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        _inDialogue = false;
        
        _player._playerInput.SwitchCurrentActionMap("Player");
    }


    public void TakeCollect()
    {
        _takeCollect ++;
        _player._animator.SetTrigger("Take");
    }

    public void UpdateCollect()
    {
        //_animator.GetComponentInParent<InteractableManager>()._animator.SetBool("Open");
        if (_takeCollect == 4f)
        {
            
        }
        
        
    }
}
