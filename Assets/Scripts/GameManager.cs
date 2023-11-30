using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using FMOD.Studio;
using FMODUnity;
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
using UnityEngine.UIElements;
using UnityEngine.WSA;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;



public class GameManager : MonoBehaviour
{
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    public PlayerController _player;
    [SerializeField] private CinemachineInputProvider _playerCamInputProvider;
    private InteractableManager _interactableManager;
    public EventReference _musicEventReference;
    private EventInstance _musicEventInstance;

    [Header("Interact")] 
    [SerializeField] private GameObject _InteractUI;
    public float _takeCollect = 0f;
    [Header("Dialouge")]
    public bool _inUI = false;
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
    [Header("Buttons for Dialog")]
    [SerializeField] private GameObject _buttonGroup;
    [SerializeField] private GameObject _buttonCurrent;
    [Header("ShowQuest")] 
    [SerializeField] private GameObject _QuestUI;
    [SerializeField] private TextMeshProUGUI _TextQuest;
    [SerializeField] private Button _QuestButton;
    [SerializeField] public Animator _aniDialogRosie;
    [Header("Ende")] 
    [SerializeField] private GameObject _EndeUI;
    [SerializeField] private Button _EndeButton;
    [Header("Tab")] 
    public List<string> _importantItems;
    [SerializeField] private GameObject _Quest1;
    [SerializeField] private TextMeshProUGUI _Tabtext1;
    [SerializeField] private GameObject _Quest2;
    [SerializeField] private GameObject _Quest3;
    [SerializeField] private GameObject _Quest4;
    [SerializeField] private GameObject _Quest5;
    [SerializeField] private GameObject _Quest6;

    
    
    //DialogManager
    private Dialog _currentLines;
    private int _currentLineIndex;
    //QuestManager
    private QuestManagerLine _currentLineQuest;
    private QuestManager _questManager;
    

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

    private void Update()
    {
        if (_takeCollect == 5f) 
        { 
            _Quest1.GetComponentInChildren<Collider>(CompareTag("Quest 1")).enabled = false; 
            _Quest1.GetComponentInChildren<GameObject>(CompareTag("Emission")).SetActive(false);
            _importantItems.Add("ReadyQuest1");
        }
    }

    //InteractUI
    public void ShowIneractUI(bool show)
    {
        _InteractUI.SetActive(show);
    }
    
    
    //DialogUI
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
        ClearButton();
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
        dialogueLines._lineEvent.Invoke();
        foreach (Buttons buttons in dialogueLines._Buttons)
        {
            GameObject buttonInstance = Instantiate(_buttonCurrent, _buttonGroup.transform);
            buttonInstance.GetComponent<ButtonManager>().Setup(buttons._text, buttons._buttonEvent);
        }
        StartCoroutine(FocusButton(dialogueLines));
    }
    IEnumerator FocusButton(DialoguesLines dialoguesLines)
    {
       yield return new WaitForEndOfFrame();
       //first Dialog Button
       if (dialoguesLines._Buttons.Count > 0)
       {
           GameObject firstButtom = _buttonGroup.transform.GetChild(0).gameObject;
           firstButtom.GetComponent<Button>().Select();
           _DialogButton.gameObject.SetActive(false);
       }
       else 
       {
           _DialogButton.gameObject.SetActive(true);
           _DialogButton.Select();
       }
    }
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
        _aniDialogRosie.Play("DialogUIRosieScaleLow");
    } 
    public void AnimationEventCloseDialogUI()
    { 
        //Player
        _player._playerInput.SwitchCurrentActionMap("Player"); 
        _playerCamInputProvider.enabled = true; 
        Cursor.lockState = CursorLockMode.Locked;
        //GameManager
        _DialogUI.SetActive(false);
        _inUI = false;
        _currentLines.dialogEnd.Invoke();
        _currentLines.GetComponentInParent<DialoguesManager>()._dialogCam.Priority = 0;
        ClearButton();
    }
    private void ClearButton()
    {
        foreach (Transform t in _buttonGroup.transform)
        {
            Destroy(t.gameObject);
        }
    }
    public void ShowNewDialog(Dialog dialog)
    {
        _currentLines = dialog;
        _currentLineIndex = 0;
        ShowCurrentLine();
    }

    
    
    
    //QuestUI
    public void ShowQuestUI()
    {
        ShowIneractUI(false);
        //Player
        _player._playerInput.SwitchCurrentActionMap("UI");
        _playerCamInputProvider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined; //Maus
        //GameManager
        _QuestUI.SetActive(true);
        QuestManagerLine questManagerLine = _currentLineQuest;
        _TextQuest.SetText(questManagerLine._Questtext);
        _QuestButton.Select();
    }
    public void CloseQuestUI()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("Player");
        _playerCamInputProvider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        //Quest
        _QuestUI.SetActive(false);
        _Tabtext1.SetText(_currentLineQuest._Questtext);
        _questManager._questCam.Priority = 0;
    }
    
    
    
    //ENDE
    public void ShowEndUI()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("UI");
        _playerCamInputProvider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined; //Maus
        //Ende
        _EndeUI.SetActive(true);
        _inUI = true;
        ShowIneractUI(false);
        _EndeButton.Select();
    }
    public void CloseEndeUI()
    {
        SceneManager.LoadScene("Paychatrie");
    }
    
    
    
    //Quest1
    public void TakeCollectQuest1()
        {
            //Player
            _player._animator.SetTrigger("Take");
            _player._playerInput.SwitchCurrentActionMap("UI");
            _playerCamInputProvider.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            //GameManager
            _Tabtext1.SetText("[0]/5)", _takeCollect);
            ShowIneractUI(false);
            _takeCollect++;
        }
    
    public void DestroyCollect()
    {
        foreach (Transform t in _player._currentInteractable.transform);
        Debug.Log(gameObject);
        {
            
        }
        _Quest1.GetComponentInChildren<InteractableManager>(CompareTag("Quest 1")).enabled = false; 
        _Quest1.GetComponentInChildren<InteractableManager>(CompareTag("Emission")).enabled = (false);
        //foreach (Transform t in _Quest1.transform) { Destroy(t.gameObject); }
       
        
        //_player._playerInput.SwitchCurrentActionMap("Player");
    } 
    
    
    
    //ImportantItem
    public void AddItem(string id)
    { 
        _importantItems.Add(id); 
    }
    public void RemoveItem(string id)
    { 
        _importantItems.Remove(id);
    }
}
