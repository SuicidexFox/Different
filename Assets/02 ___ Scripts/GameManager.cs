using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController playerController;
    public MainMenu menu;
    public QuestLog questLog;
    private Dialog currentLines;
    public string scenesManager;
    
    
    private void Awake() { instance = this; }
    private void Start()
    {
        scenesManager = SceneManager.GetActiveScene().name;
        if (scenesManager == "Psychiatry")
        {
            playerController.DeactivateInput();
            playerController.animator.Play("Psychiatry"); 
            StartCoroutine(WaitPsychiatry());
        }
        else { playerController.DeactivateInput(); }
        StartCoroutine(WaitStart()); 
        
    }
    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(1); 
        playerController.ActivateInput();
        menu.fade.SetActive(false);
    }
    IEnumerator WaitPsychiatry()
    {
        yield return new WaitForSeconds(1); 
        dialogPsychiatry.ShowDialogue();
        menu.fade.SetActive(false);
    }

    ///////////////////////////////////// DialogUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    [Header("Dialog")]
    public bool inUI;
    private int currentLineIndex = 0;
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private Image character;
    [SerializeField] private Image textBox;
    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private TextMeshProUGUI textDialogNPC;
    [SerializeField] private Animator animationCloseDialog;
    [Header("Buttons for Dialog")]
    [SerializeField] private Button buttonDialog;
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject buttonCurrent;
        
    public void ShowDialogUI(Dialog dialog)
    {
        playerController.DeactivateInput();
        Cursor.SetCursor(menu.cursorHand, Vector2.zero, CursorMode.ForceSoftware);
        dialogUI.SetActive(true);
        inUI = true;
        
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
        DialoguesLines dialogueLines = currentLines.DialoguesLinesList[currentLineIndex];
        ClearButton();
        if (dialogueLines == null) { CloseDialogUI(); }
        if (dialogueLines.sound.Path != null) { RuntimeManager.PlayOneShot(dialogueLines.sound); }
        character.sprite = dialogueLines.character;
        textBox.sprite = dialogueLines.textBox;
        textDialog.SetText(dialogueLines.text);
        textDialogNPC.SetText(dialogueLines.textNpc);
        dialogueLines.lineEvent.Invoke();
        foreach (ButtonLines buttonsLines in dialogueLines.buttonLinesList)
        {
            GameObject buttonInstance = Instantiate(buttonCurrent, buttonGroup.transform);
            buttonInstance.GetComponent<CurrentButton>().Setup(buttonsLines.text, buttonsLines.buttonEvent);
        }
        StartCoroutine(SelectButtonDialogUI(dialogueLines));
    }
    IEnumerator SelectButtonDialogUI(DialoguesLines dialoguesLines)
    {
       yield return new WaitForEndOfFrame();
       if (dialoguesLines.buttonLinesList.Count > 0)
       {
           GameObject firstButtom = buttonCurrent.transform.GetChild(0).gameObject;
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
        if (!inUI) { return; }
        currentLineIndex++; //einfach immer eins weiter zählen
        if (currentLines.DialoguesLinesList.Count == currentLineIndex)
        {
            CloseDialogUI();
            return;
        }
        ShowCurrentLine();
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
        Cursor.SetCursor(menu.cursorNull, Vector2.zero, CursorMode.ForceSoftware);
    } 
    public void AnimationEventCloseDialogUI()
    { 
        playerController.ActivateInput();
        //GameManager
        dialogUI.SetActive(false);
        inUI = false;
        currentLines.dialogEnd.Invoke();
        currentLines.GetComponentInParent<DialogManager>().dialogCam.Priority = 0;
        ClearButton();
    }
    private void ClearButton()
    {
        foreach (Transform t in buttonGroup.transform)
        {
            Destroy(t.gameObject);
        }
    }
    
    
    ///////////////////////////////////// QuestUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    [Header("ShowQuest")] 
    [SerializeField] private GameObject questUI;
    [SerializeField] private TextMeshProUGUI textQuest;
    [SerializeField] private Button buttonQuest;
    [SerializeField] private Animator animationCloseQuestUI;
    public void ShowQuestUI(QuestManager questManager)
    {
        playerController.DeactivateInput();
        ShowIneractUI(false);
        inUI = true;
        questUI.SetActive(true);
        textQuest.SetText(questManager.text);
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Collect/Take 4");
    }
    public void AnimationSelectButtonQuestUI() { buttonQuest.Select(); }
    public void CloseQuestUI()
    {
        animationCloseQuestUI.Play("QuestUIScaleSmal");
        Cursor.SetCursor(menu.cursorNull, Vector2.zero, CursorMode.ForceSoftware);
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/QuestUI/QuestUIClose");
    }
    public void AnimationEventCloseQuestUI()
    {
        playerController.ActivateInput();
        questUI.SetActive(false);
        inUI = false;
    }
    
    
    ///////////////////////////////////// InteractUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    [Header("Interact")] 
    [SerializeField] private GameObject interactUI;
    
    public float dishes;
    public float rorschach;
    public DialogManager dialogPsychiatry;
    public float skillkit;
    public float letter;
    public List<string> importantItems;
    public void ShowIneractUI(bool show)
    {
        interactUI.SetActive(show);
    }
    public void QuestInteractables()
    {
        ShowIneractUI(false);
        playerController.animator.SetTrigger("Take");
        playerController.Emote();
        if (playerController.currentInteractable.CompareTag("Dishes"))
        { 
            
            dishes++;
            if (dishes == 5f)
            {
                importantItems.Add("Dishes");
                dishes = 6f;
                questLog.Questende();
                questLog.lettertext.fontStyle = FontStyles.Strikethrough;
                questLog.GetComponentInChildren<Animator>().enabled = true;
            }
        }
        if (playerController.currentInteractable.gameObject.CompareTag("Rorschachtest"))
        {
            rorschach++;
            if (rorschach == 4f)
            {
                importantItems.Add("Rorschach"); ;
                rorschach = 5f;
                questLog.Questende();
                questLog.GetComponentInChildren<Animator>(CompareTag("Rorschachtest")).enabled = true;
            }
        }
        if (playerController.currentInteractable.CompareTag("Skillkit"))
        {
            skillkit++;
            if (skillkit == 3f)
            {
                importantItems.Add("Skillbag");
                skillkit = 4f;
                questLog.Questende();
                questLog.lettertext.fontStyle = FontStyles.Strikethrough;
                questLog.GetComponentInChildren<Animator>().enabled = true;
                questLog.GetComponentInChildren<Animator>(CompareTag("Skillkit")).enabled = true;
            }
        }
        if (playerController.currentInteractable.CompareTag("Letter"))
        { 
            letter++; 
            if (letter == 2f) 
            { 
                importantItems.Add("Letter");
                letter = 3f;
                questLog.Questende();
                questLog.lettertext.fontStyle = FontStyles.Strikethrough;
                questLog.GetComponentInChildren<Animator>().enabled = true;
            }
        }
    }
    public void DestroyInteractable()
    {
        if (playerController.currentInteractable != null)
        { Destroy(playerController.currentInteractable.gameObject); }
    }
    
    
    
    ///////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    [Header("Pause")] 
    public bool pause;
    [SerializeField] private GameObject pauseUI;
    
    public void TogglePause() 
    {
        if (inUI == true) { return; }
        pause = !pause;
        pauseUI.SetActive(pause);
            
        if (pause)
        { 
            playerController.DeactivateInput();
            RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSidePause");
            menu.buttonMain.Select();
            Time.timeScale = 0.0f;
        }
        else
        { 
            playerController.ActivateInput();
            RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/ClosePause");
            menu.main.SetActive(true);
            menu.settings.SetActive(false);
            menu.sound.SetActive(false);
            menu.controls.SetActive(false);
            Time.timeScale = 1.0f;
        } 
    }
}
