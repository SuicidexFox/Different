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
    public float _dishes = 0f;
    public float _rorschach = 0f;
    public float _skillbag = 0f;
    public float _letter = 0f;
    public List<string> _importantItems;
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
    [SerializeField] private Button _ButtonQuestUI;
    [SerializeField] private GameObject _TabUI;
    [Header("Fade")] 
    [SerializeField] private GameObject _FadeOut;

    
    
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
        _FadeOut.SetActive(false);
        _musicEventInstance = RuntimeManager.CreateInstance(_musicEventReference);
        _musicEventInstance.start();
        _musicEventInstance.setParameterByName("MusicStage", 2);
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
        _TabUI.SetActive(false);
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
       if (dialoguesLines._Buttons.Count > 0) //first Dialog Button
       {
           GameObject firstButtom = _buttonGroup.transform.GetChild(0).gameObject;
           firstButtom.GetComponent<Button>().Select();
           _ButtonDialog.gameObject.SetActive(false);
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
        _TabUI.SetActive(true);
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
        _ButtonQuestUI.Select();
        _TabUI.SetActive(false);
    }
    public void CloseQuestUI()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("Player");
        _playerCamInputProvider.enabled = true;
        //Quest
        _QuestUI.SetActive(false);
        _TabUI.SetActive(true);
        _player._currentInteractable = null;
    }
    
    
    
    //FadeOut
    public void ShowFadeOut()
    {
        //Player
        _player._playerInput.SwitchCurrentActionMap("UI");
        _playerCamInputProvider.enabled = false;
        //Ende
        _FadeOut.SetActive(true);
        _TabUI.SetActive(false);
        ShowIneractUI(false);
    }
    public void CloseFadeOut() { SceneManager.LoadScene("Psychatrie"); }

    public void SavePlace()
    { SceneManager.LoadScene("Save Place"); }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(10);
        Application.Quit();
    }
    
    
    //Quest1
    public void QuestInteractables(InteractableManager interactableManager)
        {
            //Player
            _player._animator.SetTrigger("Take");  
            _player._playerInput.SwitchCurrentActionMap("UI");
            _playerCamInputProvider.enabled = false;
            //GameManager
            ShowIneractUI(false);
            
            //Abfrage der Interactables
            if (interactableManager._dishes == true) { _dishes++; }
            if (interactableManager._rorschach == true) { _rorschach++; }
            if (interactableManager._skillbag == true) { _skillbag++; }
            if (interactableManager._letter == true) { _letter++; }
            else {return;}
        }
    public void DestroyInteractable()
    {
        if (_player._currentInteractable != null)
        {
            Destroy(_player._currentInteractable.GameObject());
        }
        _player._playerInput.SwitchCurrentActionMap("Player");
        _playerCamInputProvider.enabled = true;
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
