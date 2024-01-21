using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{
        ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public static GameManager instance;
    public PlayerController playerController;
    public Menu menu;
    private EventInstance musicEventInstance;
    private Interactable interactableManager;
    private Dialog currentLines;
    private QuestManager questManager;
    
    
    [Header("Interact")] 
    [SerializeField] private GameObject interactUI;
    public float dishes;
    public float rorschach;
    public GameObject dialogPsychiatry;
    public float skillkit;
    public float letter;
    public List<string> importantItems;
    [Header("Dialog")]
    public bool inUI;
    private int currentLineIndex;
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
    [Header("ShowQuest")] 
    [SerializeField] private GameObject questUI;
    [SerializeField] private TextMeshProUGUI textQuest;
    [SerializeField] private Button buttonQuest;
    [SerializeField] private Animator animationCloseQuestUI;
    [SerializeField] private GameObject questLog;
    [Header("Fade")] 
    [SerializeField] public GameObject fade;
    [SerializeField] private Animator animationFade;
    [SerializeField] private GameObject fadeOutLetter;
    [SerializeField] private Button buttonFadeOut;
    [Header("Pause")] 
    public bool pause;
    [SerializeField] private GameObject pauseUI;
    
    
          ///////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Awake() //firstStart this Game
    {
        instance = this;
        
    }
    private void Start()
    {
        playerController.DeactivateInput();
        musicEventInstance.setParameterByName(menu.musicEventReference.Path, 0);
        animationFade.Play("FadeIn");
    }
         ///////////////////////////////////// FadeIn \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
         public void PlayerPlay()
         {
             //_player.ActivateInput(); 
             //if (_menu._scenesManager == "Kitchen") { Kitchen(); }
             //if (_menu._scenesManager == "Psychiatry") { Psychiatry(); }
         }
    
    
    
    
    
    
    
    
         ///////////////////////////////////// Interact \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowIneractUI(bool show)
    {
        interactUI.SetActive(show);
    }
    public void QuestInteractables()
    {
        playerController.animator.SetTrigger("Take");  
        playerController.playerInput.SwitchCurrentActionMap("UI");
        playerController.playerCamInputProvider.enabled = false;
        ShowIneractUI(false);
        if (playerController.currentInteractable.CompareTag("Dishes"))
        {
            dishes++;
            if (dishes == 5f)
            {
                importantItems.Add("Dishes");
                dishes = 6f;
                questManager.DestroyInteractable();
            }
        }
        if (playerController.currentInteractable.CompareTag("Rorschachtest"))
        {
            rorschach++;
            if (rorschach == 4f)
            {
                importantItems.Add("Rorschach");
                rorschach = 5f;
            }
        }
        if (playerController.currentInteractable.CompareTag("Skillkit"))
        {
            skillkit++;
            if (skillkit == 3f)
            {
                importantItems.Add("Skillbag");
                skillkit = 4f;
            }
        }
        if (playerController.currentInteractable.CompareTag("Letter"))
        { 
            letter++; 
            if (letter == 2f) 
            { 
                importantItems.Add("Letter"); 
                letter = 3f;
            }
        }
    }
    public void DestroyInteractable()
    {
        if (playerController.currentInteractable != null)
        {
                Destroy(playerController.currentInteractable.GameObject());
        }
        playerController.playerInput.SwitchCurrentActionMap("Player");
        playerController.playerCamInputProvider.enabled = true;
    }
    
        ///////////////////////////////////// DialogUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowDialogUI(Dialog dialog)
    {
        musicEventInstance.setParameterByName(menu.musicEventReference.Path, 2);
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
        if (dialogueLines == null) { return; }
        ClearButton();
        RuntimeManager.PlayOneShot(dialogueLines.sound);
        character.sprite = dialogueLines.character;
        textBox.sprite = dialogueLines.textBox;
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
           GameObject firstButtom = buttonGroup.transform.GetChild(0).gameObject;
           firstButtom.GetComponent<Button>().Select();
           buttonDialog.gameObject.SetActive(false);
       }
       else 
       {
           buttonGroup.SetActive(false);
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
        musicEventInstance.setParameterByName(menu.musicEventReference.Path, 2);
        Cursor.SetCursor(menu.cursorNull, Vector2.zero, CursorMode.ForceSoftware);
    } 
    public void AnimationCloseDialogUI()
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
    
    
    ///////////////////////////////////// TypewriterEffect \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\  
    private float speed = 0.05f;
    private int currentPosition = -1;
    private bool _hasFinished { get; set; }
    public void RunTypewriterEffect()
    {
        DialoguesLines dialogueLines = currentLines.DialoguesLinesList[currentLineIndex];
        textDialog.SetText(dialogueLines.text);
        textDialogNPC.SetText(dialogueLines.textNpc);
        StartCoroutine(TypewriterEffect());
    }
    IEnumerator TypewriterEffect()
    { 
        var textLenght = textDialog.text.Length;
        while (!_hasFinished && currentPosition + 1 < textLenght)
        {
            textDialog.text += GetNextToken();
            yield return new WaitForSeconds(speed);
        }
        _hasFinished = true;
    }
    private string GetNextToken()
    {
        currentPosition++;
        var nextToken = textDialog.text[currentPosition].ToString();
        return nextToken;
    } 
    

         ///////////////////////////////////// QuestUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowQuestUI(QuestManager questManager)
    {
        playerController.DeactivateInput();
        Cursor.SetCursor(menu.cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
        ShowIneractUI(false);
        inUI = true;
        questUI.SetActive(true);
        textQuest.SetText(questManager.text);
        questLog = Instantiate(questManager.currentLetter);
    }
    public void AnimationSelectButtonQuestUI()
    {
        buttonQuest.Select();
    }
    public void CloseQuestUI()
    {
        animationCloseQuestUI.Play("QuestUIScaleSmal");
        Cursor.SetCursor(menu.cursorNull, Vector2.zero, CursorMode.ForceSoftware);
    }
    public void AnimationCloseQuestUI()
    {
        playerController.ActivateInput();
        questUI.SetActive(false);
        inUI = false;
        questManager.GetComponent<QuestManager>().questCam.Priority = 0;
    }
    
    
          ///////////////////////////////////// FadeOut \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ShowFadeOut()
    { 
        playerController.DeactivateInput();
        Cursor.SetCursor(menu.cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
        ShowIneractUI(false);
        fade.SetActive(true);
        if (menu.scenesManager == "Kitchen")
        { 
            fadeOutLetter.SetActive(true);
            animationFade.Play("FadeOut");
            return;
        }
        animationFade.Play("FadeOutShort");
    } 
    public void AnumationSelectButtonFadeOutUI()
    {
            buttonFadeOut.Select();
    }
    public void Scenes()
    {
        if (menu.scenesManager == "Kitchen") { SceneManager.LoadScene("Psychiatry"); }
        if (menu.scenesManager == "Psychiatry") { SceneManager.LoadScene("Save Place"); }
        if (menu.scenesManager == "Save Place") { SceneManager.LoadScene("Credits"); }
        if (menu.scenesManager == "Credits") { SceneManager.LoadScene("MainMenu"); }
        menu.StopMusic();
    }
    
    
    ///////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void TogglePause() 
    {
        if (inUI == true) { return; }
        pause = !pause;
        pauseUI.SetActive(pause);
            
        if (pause)
        {
            playerController.DeactivateInput();
            Cursor.SetCursor(menu.cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
            Time.timeScale = 0.0f;
        }
        else
        { 
            playerController.ActivateInput();
            Cursor.SetCursor(menu.cursorNull, Vector2.zero, CursorMode.ForceSoftware);
            Time.timeScale = 1.0f;
        } 
    }

    public bool emote;
    public void ToggleEmote()
    {
        if (inUI == true) { return; }

        emote = !emote;
        if (emote)
        { 
            musicEventInstance.setParameterByName("event:/SFX/Rosie/Emotes/Cry", 2);
            playerController.DeactivateInput();
        }
        else
        { 
            playerController.ActivateInput();
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
