using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AI;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.WSA;
using Cursor = UnityEngine.Cursor;




[Serializable]
public class Collectabels
{
    public GameObject _Collect;
    public UnityEvent SetActive;
}


public class GameManager : MonoBehaviour
{
    public List<GameObject> _collectable;
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    public PlayerController _player;
    [SerializeField] private CinemachineInputProvider _playerCamInputProvider;
    private InteractableManager _interactableManager;
    public bool _inUI = false;
    

    [Header("Interact")] 
    [SerializeField] private GameObject _InteractUI;
    public float _takeCollect = 0f;
    [Header("Dialouge")] 
    [SerializeField] public GameObject _DialogUI;
    [SerializeField] private GameObject _Rosie;
    [SerializeField] private GameObject _ChatboxTalk;
    [SerializeField] private GameObject _ChatboxThink;
    [SerializeField] private TextMeshProUGUI _TextDialog;
    [SerializeField] private TextMeshProUGUI _TextDialogNPC;
    [SerializeField] private Button _DialogButton;
    [SerializeField] private GameObject _Therapist;
    [SerializeField] private GameObject _Ergo;
    [SerializeField] private GameObject _InnerChilde;
    [Header("ShowQuest")] 
    [SerializeField] private GameObject _QuestUI;
    [SerializeField] private TextMeshProUGUI _TextQuest;
    [SerializeField] public Animator _aniDialogRosie;

    
    
    [Header("Ende")] 
    [SerializeField] private GameObject _EndeUI;

    [SerializeField] private TextMeshProUGUI _TextEnde;
    [SerializeField] private Button _EndeButton;
    
    //DialogManager
    private Dialog _currentLines;
    private int _currentLineIndex;
    //QuestManager
    private QuestManager _currentQuestLine;
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
        //_quest1.GetComponentInChildren<Collider>(CompareTag("Quest 1")).enabled = false;
    }

    //Anzeige der Tasten und Hinweise vllt. Tutorial
    public void ShowIneractUI(bool show)
    {
        _InteractUI.SetActive(show);
    }


    
    //Dialog
    public void ShowDialogUI(Dialog dialog)
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("UI");
        _playerCamInputProvider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined; //Maus
        //Dialog
        _DialogUI.SetActive(true);
        _inUI = true;
        _TextDialog.SetText("");
        _currentLines = dialog;
        _currentLineIndex = 0;
        ShowIneractUI(false);
        ShowCurrentLine();
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
    }
    //IEnumerator FocusButton()
    public void NextDialogLine()
    {
        if (!_inUI) { return; }
        _currentLineIndex++; //einfach immer eins weiter zählen
        if (_currentLines.dialoguesLines.Count == _currentLineIndex)
        {
            CloseDialogUI();
            return;
        }

        ShowCurrentLine();
    }
    public void CloseDialogUI()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("Player");
        _playerCamInputProvider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        //Dialog
        _aniDialogRosie.Play("DialogUIRosieScaleLow");
        _inUI = false;
        _currentLines.dialogEnd.Invoke();
        _currentLines.GetComponentInParent<DialoguesManager>()._dialogCam.Priority = 0;
    }

    public void AnimationEventCloseDialogUI()
    {
        _DialogUI.SetActive(false);
    }
    
    
    
    //Quest
    public void ShowQuestUI(QuestManager questManager)
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("UI");
        _playerCamInputProvider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined; //Maus
        //Quest
        _QuestUI.SetActive(true);
        _TextQuest.SetText("");
        _currentQuestLine = questManager;
        _currentQuestLineIndex = 0;
        ShowIneractUI(false);
        ShowCurrentQuestLine();
    }
    public void ShowCurrentQuestLine()
    {
        Questlines questlines = _currentQuestLine._questlines[_currentQuestLineIndex];
        if (questlines == null) { return; }
        _TextQuest.SetText(questlines._Questtext);
    } 
    public void CloseQuestUI()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("Player");
        _playerCamInputProvider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        //Quest
        _QuestUI.SetActive(false);
        _currentQuestLine._questCam.Priority = 0;
    }
    
    
    
    //ENDE
    public void ShowEndUI(Dialog dialog)
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("UI");
        _playerCamInputProvider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined; //Maus
        //Ende
        _EndeUI.SetActive(true);
        _inUI = true;
        _TextQuest.SetText("");
        _currentLines = dialog;
        _currentLineIndex = 0;
        ShowIneractUI(false);
        ShowCurrentEndeLine();
    }
    public void ShowCurrentEndeLine()
    {
        DialoguesLines dialogueLines = _currentLines.dialoguesLines[_currentLineIndex];
        if (dialogueLines == null) { return; }
        _TextEnde.SetText(dialogueLines._text);
    }
    public void CloseEndeUI()
    {}
    
    
    
    
    //Ineract
    public void TakeCollect()
        {
            _player._animator.SetTrigger("Take");
            ShowIneractUI(false);
            _player._playerInput.DeactivateInput();
            _player._currentInteractable = null;
            if (_takeCollect == 5f)
            {
            }
            else
            {
                _takeCollect++;
            }
        }
}
