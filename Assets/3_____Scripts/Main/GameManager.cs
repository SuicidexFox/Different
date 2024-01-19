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
           ///////////////////////////////////// Variablen \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public static GameManager instance; //"static" macht es nur einmahlig und kann überall aufgerufen werden
    private string sceneManager;
    public PlayerController _player;
    private InteractableManager interactableManager;
    private Dialog currentLines;
    private QuestManager questManager;
    private EventInstance _musicEventInstance;
    public EventReference musicEventReference;
    
    [Header("Interact")] 
    [SerializeField] private GameObject interactUI;
    public float _dishes = 0f;
    public float _rorschach = 0f;
    public float _skillkit = 0f;
    public float _letter = 0f;
    public List<string> _importantItems;
    [Header("Dialouge")]
    public bool _inUI = false;
    private int currentLineIndex;
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private Image character;
    [SerializeField] private Image textbox;
    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private TextMeshProUGUI textDialogNPC;
    [SerializeField] private Animator animationCloseDialog;
    [Header("Buttons for Dialog")]
    [SerializeField] private Button buttonDialog;
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject buttonCurrent;
    [Header("ShowQuest")] 
    [SerializeField] private GameObject questUI;
    [SerializeField] private TextMeshProUGUI textQuest;
    [SerializeField] private Button buttonQuest;
    [SerializeField]private Animator animationCloseQuestUI;
    [Header("Fade")] 
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private Animator animationFadeIn;
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private Button buttonFadeOut;
    [SerializeField] private Animator animationFadeOut;
    [Header("Pause")] 
    public bool _pause = false;
    [SerializeField] private GameObject pauseUI;
    
    
          ///////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Awake() //nur beim ersten Start der gesamten Instanz
    {
        instance = this;
        
    }
    private void Start()
    {
        sceneManager = SceneManager.GetActiveScene().name;
        _musicEventInstance = RuntimeManager.CreateInstance(musicEventReference);
        _musicEventInstance.start();
        _musicEventInstance.setParameterByName("event:/Music/Kitchen", 0);
        _player.DeactivateInput();
        
        fadeIn.SetActive(true);
        animationFadeIn.SetTrigger("FadeIn");
    }
         ///////////////////////////////////// FadeIn \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationPlay() 
    { 
        _player.ActivateInput();
    }
    
         ///////////////////////////////////// Interact \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowIneractUI(bool show)
    {
        interactUI.SetActive(show);
    }
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
    
        ///////////////////////////////////// DialogUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowDialogUI(Dialog dialog)
    {
        _musicEventInstance.setParameterByName("event:/Music/Kitchen", 2);
        _player.DeactivateInput();
        dialogUI.SetActive(true);
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
        dialogueLines._lineEvent.Invoke();
        foreach (Buttons buttons in dialogueLines._Buttons)
        {
            GameObject buttonInstance = Instantiate(buttonCurrent, buttonGroup.transform);
            buttonInstance.GetComponent<ButtonManager>().Setup(buttons._text, buttons._buttonEvent);
            buttonDialog.gameObject.SetActive(false);
        }
        StartCoroutine(SelectButtonDialogUI(dialogueLines));
    }
    IEnumerator SelectButtonDialogUI(DialoguesLines dialoguesLines)
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
        RunTypewriterEffect();
    }
    public void ShowNewDialog(Dialog dialog)
    {
        currentLines = dialog;
        currentLineIndex = 0;
        ShowCurrentLine();
    }
    public void CloseDialogUI()
    {
        animationCloseDialog.Play("DialogUIScaleLow");
        _musicEventInstance.setParameterByName("event:/Music/Kitchen", 0);
    } 
    public void AnimationCloseDialogUI()
    { 
        _player.ActivateInput();
        //GameManager
        dialogUI.SetActive(false);
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
    
    
    ///////////////////////////////////// TypewriterEffect \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\  
    private float _speed = 0.05f;
    private int _currentPosition = -1;
    private bool _hasFinished { get; set; }
    public void RunTypewriterEffect()
    {
        DialoguesLines dialogueLines = currentLines.dialoguesLines[currentLineIndex];
        textDialog.SetText(dialogueLines._text);
        textDialogNPC.SetText(dialogueLines._textNPC);
        StartCoroutine(TypewriterEffect());
    }
    IEnumerator TypewriterEffect()
    { 
        var textLenght = textDialog.text.Length;
        while (!_hasFinished && _currentPosition + 1 < textLenght)
        {
            textDialog.text += GetNextToken();
            yield return new WaitForSeconds(_speed);
        }
        _hasFinished = true;
    }
    private string GetNextToken()
    {
        _currentPosition++;
        var nextToken = textDialog.text[_currentPosition].ToString();
        return nextToken;
    } 
    

         ///////////////////////////////////// QuestUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowQuestUI(QuestManager questManager)
    {
        _player.DeactivateInput();
        ShowIneractUI(false);
        _inUI = true;
        questUI.SetActive(true);
        textQuest.SetText(questManager._text);
        Instantiate(questManager._currentLetter, _player._tabUIGroupe.transform);
    }
    public void AnimationSelectButtonQuestUI()
    {
        buttonQuest.Select();
    }
    public void CloseQuestUI()
    {
        animationCloseQuestUI.Play("QuestUIScaleSmal");
    }
    public void AnimationCloseQuestUI()
    {
        _player.ActivateInput();
        questUI.SetActive(false);
        _inUI = false;
    }
    
    
          ///////////////////////////////////// FadeUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowFadeOut()
    { 
        _player.DeactivateInput();
        ShowIneractUI(false);
        fadeOut.SetActive(true);
        animationFadeOut.Play("FadeOut");
    } 
    public void AnumationSelectButtonFadeOutUI()
    {
            buttonFadeOut.Select();
    }
    public void Scenes()
    {
        if (sceneManager == "Kitchen")
        {
            SceneManager.LoadScene("Psychatrie");
            _musicEventInstance.setVolume(0);
            return;
        }
        if (sceneManager == "Psychatrie") { SceneManager.LoadScene("Save Place"); }
        if (sceneManager == "Save Place") { SceneManager.LoadScene("Abspann"); }
        if (sceneManager == "Abspann") { SceneManager.LoadScene("MainMenu"); }
    }
    
    
    ///////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void TogglePause() 
    {
        if (_inUI == true) { return; }
        _pause = !_pause;
        pauseUI.SetActive(_pause);
            
        if (_pause)
        {
            _player.DeactivateInput();
            Time.timeScale = 0.0f;
        }
        else
        { 
            _player.ActivateInput(); 
            Time.timeScale = 1.0f;
        } 
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
