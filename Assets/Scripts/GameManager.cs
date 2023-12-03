using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    [SerializeField] private Button _ButtonDialog;
    [SerializeField] private GameObject _Therapist;
    [SerializeField] private GameObject _Ergo;
    [SerializeField] private GameObject _InnerChilde;
    [SerializeField] private Animator _animationCloseDialog;
    [Header("Buttons for Dialog")]
    [SerializeField] private GameObject _buttonGroup;
    [SerializeField] private GameObject _buttonCurrent;
    [Header("ShowQuest")] 
    [SerializeField] private GameObject _QuestUI;
    [Header("Ende")] 
    [SerializeField] private GameObject _EndeUI;
    [Header("Tab")] 
    public List<string> _importantItems;

    
    
    //DialogManager
    private Dialog _currentLines;
    private int _currentLineIndex;
    

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
        _musicEventInstance = RuntimeManager.CreateInstance(_musicEventReference);
        _musicEventInstance.start();
        _musicEventInstance.setParameterByName("MusicStage", 2);
    }

    private void Update()
    {
        if (_takeCollect == 5f)
        {
            ShowEndUI();
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
        _musicEventInstance.setParameterByName("MusicStage", 1);
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
           _ButtonDialog.gameObject.SetActive(false);
           GameObject firstButtom = _buttonGroup.transform.GetChild(0).gameObject;
           firstButtom.GetComponent<Button>().Select();
       }
       else 
       {
           _ButtonDialog.gameObject.SetActive(true);
           _ButtonDialog.Select();
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
        _animationCloseDialog.Play("DialogUIScaleLow");
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
        _musicEventInstance.setParameterByName("MusicStage", 2);
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
        _playerCamInputProvider.enabled = false; //Maus
        //GameManager
        _QuestUI.SetActive(true);
    }
    public void CloseQuestUI()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("Player");
        _playerCamInputProvider.enabled = true;
        //Quest
        _QuestUI.SetActive(false);
        _player._currentInteractable = null;
    }
    
    
    
    //ENDE
    public void ShowEndUI()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("UI");
        _playerCamInputProvider.enabled = false;
        //Ende
        _EndeUI.SetActive(true);
        ShowIneractUI(false);
    }
    public void CloseEndeUI() { SceneManager.LoadScene("Psychatrie"); }
    public void SavePlace() { SceneManager.LoadScene("Save Place"); }
    public void Kitchen() { SceneManager.LoadScene("Kitchen"); }
    public void Quit() { Application.Quit();}
    
    
    
    //Quest1
    public void TakeCollectQuest1()
        {
            //Player
            _player._animator.SetTrigger("Take");
            _player._playerInput.SwitchCurrentActionMap("UI");
            _playerCamInputProvider.enabled = false;
            //GameManager
            ShowIneractUI(false);
            _takeCollect++;
        }
    public void DestroyCollect()
    {
        if (_player._currentInteractable != null)
        {
            Destroy(_player._currentInteractable.GameObject());
        }
        _player._playerInput.SwitchCurrentActionMap("Player");
        _playerCamInputProvider.enabled = true;
    }
    
    //Quest2
    


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
