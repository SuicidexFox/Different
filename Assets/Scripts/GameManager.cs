using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AI;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.WSA;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    public PlayerController _player;
    private InteractableManager _interactableManager;
    

    [Header("Interact")] 
    [SerializeField] private GameObject _InteractUI;
    public float _takeCollect = 0f;
    [Header("Dialouge")] 
    [SerializeField] private GameObject _DialogUI;
    [SerializeField] private GameObject _Rosie;
    [SerializeField] private GameObject _ChatboxTalk;
    [SerializeField] private GameObject _ChatboxThink;
    [SerializeField] private TextMeshProUGUI _TextDialog;
    [SerializeField] private GameObject _Button;
    [SerializeField] private TextMeshProUGUI _TextDialogNPC;
    [SerializeField] private GameObject _ButtonNPC;
    [SerializeField] private GameObject _Therapist;
    [SerializeField] private GameObject _Ergo;
    [SerializeField] private GameObject _InnerChilde;
    public bool _inDialogue = false;
    [Header("ShowQuest")] 
    [SerializeField] private GameObject _QuestUI;
    [SerializeField] private TextMeshProUGUI _TextQuest;
    public bool _inQuest = false;

    [Header("QuestLog")] [Header("Ende")] 
    [SerializeField] private GameObject _EndeUI;
    
    //DialogManager
    private Dialog _currentLines;
    private int _currentLineIndex;
    //QuestManager
    private Quest _currentQuestLine;
    private int _currentQuestLineIndex;

    private void Awake() //nur beim ersten Start der gesamten Instanz
    {
        instance = this;
    }

    private void Start()
    {
        _InteractUI.SetActive(false);
        _DialogUI.SetActive(false);
        _QuestUI.SetActive(false);
        _EndeUI.SetActive(false);
    }

    //Anzeige der Tasten und Hinweise vllt. Tutorial
    public void ShowIneractUI(bool show)
    {
        _InteractUI.SetActive(show);
    }


    //Dialog
    public void ShowDialogUI(bool show)
    {
        if (_currentLines == null) { return; }
        _DialogUI.SetActive(true);
    }
    public void ShowDialog(Dialog dialog)
    {
        _inDialogue = true;
        _currentLines = dialog;
        _currentLineIndex = 0;
        ShowIneractUI(false);
        _player._currentInteractable = null;
        _TextDialog.SetText("");
        Cursor.lockState = CursorLockMode.Confined;
        ShowCurrentLine();
        _player._playerInput.SwitchCurrentActionMap("UI");
    }
    private void ShowCurrentLine()
    {
        DialoguesLines dialogueLines = _currentLines.dialoguesLines[_currentLineIndex];
        if (dialogueLines == null) { return; }
        _Rosie.SetActive(dialogueLines._imageRosie);
        _ChatboxTalk.SetActive(dialogueLines._talkbox);
        _ChatboxThink.SetActive(dialogueLines._thinkbox); 
        _TextDialog.SetText(dialogueLines._text);
        _TextDialogNPC.SetText(dialogueLines._textNPC);
        _Therapist.SetActive(dialogueLines._imageTherapist);
        _Ergo.SetActive(dialogueLines._imageErgo);
        _InnerChilde.SetActive(dialogueLines._imageInnerChilde);
        _Button.SetActive(dialogueLines._button);
        _ButtonNPC.SetActive(dialogueLines._buttonNPC);
    }
    public void NextDialogLine()
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
        _currentLines.dialogEnd.Invoke();
        _currentLines.GetComponentInParent<DialoguesManager>()._dialogCam.Priority = 0;
        _DialogUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        _inDialogue = false;
        _player._playerInput.SwitchCurrentActionMap("Player");
    }
    
    //Quest
    public void ShowQuestUI(bool show)
    {
        if (_currentQuestLine == null) { return; }
        _QuestUI.SetActive(true);
        _player._playerInput.SwitchCurrentActionMap("UI");
    }
    public void ShowQuest(Quest quest)
    {
        _inQuest = true;
        _currentQuestLine = quest;
        _currentQuestLineIndex = 0;
        ShowIneractUI(false);
        _player._currentInteractable = null;
        _TextQuest.SetText("");
        Cursor.lockState = CursorLockMode.Confined;
        ShowCurrentQuestLine();
    }
    public void ShowCurrentQuestLine()
    {
        QuestsLines questLines = _currentQuestLine.questsLines[_currentQuestLineIndex];
        if (questLines == null) { return; }
        _TextQuest.SetText(questLines._Questtext);
    }
    
    public void CloseQuest()
    {
        _currentQuestLine.GetComponentInParent<QuestManager>()._questCam.Priority = 0;
        _QuestUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        _inQuest = false;
        _player._playerInput.SwitchCurrentActionMap("Player");
    }


    //Ineract
    public void TakeCollect()
        {
            _takeCollect++;
            _player._animator.SetTrigger("Take");
            ShowIneractUI(false);
            _player._playerInput.DeactivateInput();
        }
}
