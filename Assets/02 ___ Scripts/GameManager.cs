using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
        if (scenesManager == "Kitchen")
        {
            inUI = true;
            playerController.DeactivateInput();
            StartCoroutine(Kitchen());
        }
        if (scenesManager == "Psychiatry")
        {
            playerController.characterController.enabled = false;
            playerController.playerCamInputProvider.enabled = false;
            playerController.animator.Play("Psychiatry");
            StartCoroutine(WaitPsychiatry());
        }
        if (scenesManager == "SavePlace") { StartCoroutine(SavePlace()); }
    }
    IEnumerator Kitchen()
    {
        yield return new WaitForSeconds(1);
        menu.fade.SetActive(false);
        menu.tutorial.gameObject.SetActive(true);
        menu.tutorialButton.Select();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(menu.cursorHand, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void Tutorial()
    {
        menu.tutorial.gameObject.SetActive(false);
        inUI = false;
        playerController.ActivateInput();
    }
    IEnumerator WaitPsychiatry()
    {
        yield return new WaitForSeconds(1); 
        menu.fade.SetActive(false);
        playerController.currentInteractable._onInteract.Invoke();
        playerController.cinemachineBrain.m_DefaultBlend.m_Time = 4;
    }
    IEnumerator SavePlace() { yield return new WaitForSeconds(1); menu.fade.SetActive(false); }
    

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
        ClearButton();
        Cursor.SetCursor(menu.cursorHand, Vector2.zero, CursorMode.ForceSoftware);
        dialogUI.SetActive(true);
        inUI = true;
        textDialog.SetText("");
        textDialogNPC.SetText("");
        currentLines = dialog;
        currentLineIndex = 0;
        ShowIneractUI(false);
        ShowCurrentLine();
    }
    private void ShowCurrentLine()
    {
        DialoguesLines dialogueLines = currentLines.DialoguesLinesList[currentLineIndex];
        ClearButton();
        if (dialogueLines == null) { return; }
        RuntimeManager.PlayOneShot(dialogueLines.sound);
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
           buttonGroup.SetActive(true);
           GameObject firstButton = buttonGroup.transform.GetChild(0).gameObject;
           firstButton.GetComponent<Button>().Select();
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
            buttonGroup.SetActive(false);
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
    public float skillkit;
    public float saveplace;
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
                questLog.lettertext.SetText("Your ready, you can load the Dishwasher");
            }
        }
        if (playerController.currentInteractable.gameObject.CompareTag("Rorschachtest"))
        {
            rorschach++;
            if (rorschach == 4f)
            {
                importantItems.Add("Rorschach");
                rorschach = 5f;
                questLog.Questende();
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
                questLog.lettertext.SetText("Your ready, go to Mrs. Flow");
            }
        }
        if (playerController.currentInteractable.CompareTag("SavePlace"))
        {
            saveplace++;
            if (saveplace == 2f)
            {
                playerController.characterController.enabled = false;
                menu.musicInstance.setVolume(0);
                menu.musicInstance.stop(STOP_MODE.IMMEDIATE);
                playerController.cinemachineBrain.m_DefaultBlend.m_Time = 5;
                RuntimeManager.PlayOneShot("event:/SFX/Ambience/Psychiatry/Patientroom/WisperFadeOut");
                playerController.animator.Play("Cry");
                menu.fadePsychiatry.SetActive(true);
                playerController.DeactivateInput();
                StartCoroutine(CPsychiatry());
            }
        }
        if (playerController.currentInteractable.CompareTag("Letter"))
        { 
            letter++; 
            if (letter == 3f) 
            { 
                importantItems.Add("Letter");
                letter = 4f;
                questLog.Questende();
                questLog.lettertext.SetText("Perfect. Go back to your inner Child");
            }
        }
    }
    public void DestroyInteractable() { if (playerController.currentInteractable != null) { Destroy(playerController.currentInteractable.gameObject); } }
    IEnumerator CPsychiatry() { yield return new WaitForSeconds(2); menu.ScenenManager(); }
    
    
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
