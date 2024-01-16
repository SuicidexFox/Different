using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cursor = UnityEngine.WSA.Cursor;


public class GameManager : MonoBehaviour
{
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    private string sceneManager;
    public PlayerController _player;
    private InteractableManager interactableManager;
    private EventInstance musicEventInstance;
    
    [Header("Pause")] 
    public bool _pause = false;
    [SerializeField] private GameObject pauseUI;

    [Header("Interact")] 
    [SerializeField] private GameObject interactUI;
    public float _dishes = 0f;
    public float _rorschach = 0f;
    public float _skillkit = 0f;
    public float _letter = 0f;
    public List<string> _importantItems;
    [Header("Dialouge")]
    public bool _inUI = false;
    [SerializeField] public GameObject _dialogUI;
    [SerializeField] private Image character;
    [SerializeField] private Image textbox;
    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private TextMeshProUGUI textDialogNPC;
    [SerializeField] private Animator animationCloseDialog;
    [Header("Buttons for Dialog")]
    [SerializeField] private Button buttonDialog;
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject buttonCurrent;
    [Header("Fade")] 
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private GameObject fadeOutLetter;
    [SerializeField] private Button buttonFadeOut;

    
    
    //DialogManager
    private Dialog currentLines;
    private int currentLineIndex;
    

    private void Awake() //nur beim ersten Start der gesamten Instanz
    {
        instance = this;
    }

    private void Start()
    {
        sceneManager = SceneManager.GetActiveScene().name;
        interactUI.SetActive(false);
        _dialogUI.SetActive(false);
        fadeOut.SetActive(false);
        pauseUI.SetActive(false);
        musicEventInstance.setParameterByName("event:/Music/Kitchen", 2);
    }
    
  
    //InteractUI
    public void ShowIneractUI(bool show)
    {
        interactUI.SetActive(show);
    }
    //Quest
    public void QuestInteractables()
    {
        //Player
        _player._animator.SetTrigger("Take");  
        _player._playerInput.SwitchCurrentActionMap("UI");
        _player._playerCamInputProvider.enabled = false;
        //GameManager
        ShowIneractUI(false);
        //Abfrage der Interactables
        if (_player._currentInteractable.CompareTag("Dishes"))
        {
            _dishes++;
            if (_dishes == 5f)
            {
                _importantItems.Add("Dishes");
                _dishes = 6f;
            }
        }
        if (_player._currentInteractable.CompareTag("Rorschachtest"))
        {
            _rorschach++;
            if (_rorschach == 4f)
            {
                _importantItems.Add("Rorschach");
                _rorschach = 5f;
            }
        }
        if (_player._currentInteractable.CompareTag("Skillkit"))
        {
            _skillkit++;
            if (_skillkit == 3f)
            {
                _importantItems.Add("Skillbag");
                _skillkit = 4f;
            }
        }
        if (_player._currentInteractable.CompareTag("Letter"))
        { 
            _letter++; 
            if (_letter == 2f) 
            { 
                _importantItems.Add("Letter"); 
                _letter = 3f;
            }
        }
    }
    public void DestroyInteractable()
    {
        if (_player._currentInteractable != null)
        {
                Destroy(_player._currentInteractable.GameObject());
        }
        _player._playerInput.SwitchCurrentActionMap("Player");
        _player._playerCamInputProvider.enabled = true;
    }
    
    
    //DialogUI
    public void ShowDialogUI(Dialog dialog)
    {
        _player.DeactivateInput();
        //Cursor.lockState = CursorLockMode.Confined; //Maus
        //Dialog
        _dialogUI.SetActive(true);
        _inUI = true;
        textDialog.SetText("");
        textDialogNPC.SetText("");
        currentLines = dialog;
        currentLineIndex = 0;
        ShowIneractUI(false);
        ClearButton();
        ShowCurrentLine();
    }
    private void ShowCurrentLine()
    {
        DialoguesLines dialogueLines = currentLines.dialoguesLines[currentLineIndex];
        if (dialogueLines == null) { return; }
        ClearButton();
        RuntimeManager.PlayOneShot(dialogueLines._sound);
        character.sprite = dialogueLines._character;
        textbox.sprite = dialogueLines._textbox;
        textDialog.SetText(dialogueLines._text);
        textDialogNPC.SetText(dialogueLines._textNPC);
        dialogueLines._lineEvent.Invoke();
        foreach (Buttons buttons in dialogueLines._Buttons)
        {
            GameObject buttonInstance = Instantiate(buttonCurrent, buttonGroup.transform);
            buttonInstance.GetComponent<ButtonManager>().Setup(buttons._text, buttons._buttonEvent);
            buttonDialog.gameObject.SetActive(false);
        }
        StartCoroutine(DialogeLines(dialogueLines));
    }
    IEnumerator DialogeLines(DialoguesLines dialoguesLines)
    {
       yield return new WaitForEndOfFrame();
       if (dialoguesLines._Buttons.Count > 0) //first Dialog Button
       {
           GameObject firstButtom = buttonGroup.transform.GetChild(0).gameObject;
           firstButtom.GetComponent<Button>().Select();
           buttonDialog.gameObject.SetActive(false);
       }
       else 
       {
           buttonDialog.gameObject.SetActive(true);
           buttonDialog.Select();
       }
    }
    public void NextDialogLine()
    {
        if (!_inUI) { return; }
        currentLineIndex++; //einfach immer eins weiter zählen
        if (currentLines.dialoguesLines.Count == currentLineIndex)
        {
            CloseDialogUI();
            return;
        }

        ShowCurrentLine();
    }
    public void CloseDialogUI()
    {
        animationCloseDialog.Play("DialogUIScaleLow");
    } 
    public void AnimationEventCloseDialogUI()
    { 
        _player.ActivateInput();
        //GameManager
        _dialogUI.SetActive(false);
        _inUI = false;
        currentLines.dialogEnd.Invoke();
        currentLines.GetComponentInParent<DialoguesManager>()._dialogCam.Priority = 0;
        ClearButton();
    }
    private void ClearButton()
    {
        foreach (Transform t in buttonGroup.transform)
        {
            Destroy(t.gameObject);
        }
    }
    public void ShowNewDialog(Dialog dialog)
    {
        currentLines = dialog;
        currentLineIndex = 0;
        ShowCurrentLine();
    }
    

    //Pause
    public void TogglePause() 
    {
        if (_inUI == true) { return; }
        _pause = !_pause;
        pauseUI.SetActive(_pause);
            
        if (_pause)
        {
            _player.DeactivateInput();
            //Time.timeScale = 0.0f;
        }
        else
        { 
            _player.ActivateInput(); 
            //Time.timeScale = 1.0f;
        } 
    }
    
    
    //FadeOut - Lade die nächste Scene
    public void ShowFadeOut()
    { 
        if (pauseUI == true) 
        {
            TogglePause();
            fadeOutLetter.SetActive(false);
            buttonFadeOut.enabled = false;
            _player.DeactivateInput(); 
            fadeOut.SetActive(true);
        }
        else
        { 
            fadeOut.SetActive(true); 
            _player.DeactivateInput();
            ShowIneractUI(false);
            StartCoroutine(FocusFadeButton());
        }
    }
    IEnumerator FocusFadeButton()
    {
        yield return new WaitForEndOfFrame();
        buttonFadeOut.Select();   
    }
    public void CloseFadeOut()
    {
        if (sceneManager == "Kitchen") { SceneManager.LoadScene("Psychatrie"); }
        if (sceneManager == "Psychatrie") { SceneManager.LoadScene("Save Place"); }
        if (sceneManager == "Save Place") { SceneManager.LoadScene("Abspann"); }
        if (sceneManager == "Abspann") { SceneManager.LoadScene("MainMenu"); }
    }
    
    
    /*
//ImportantItem
    public void AddItem(string id)
    {
        _importantItems.Add(id);
    }
    public void RemoveItem(string id)
    {
        _importantItems.Remove(id);
    }
    */
}
