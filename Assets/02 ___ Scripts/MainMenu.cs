using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MainMenu : MonoBehaviour
{   ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public static MainMenu instance;
    public EventInstance musicEventInstance;
    public EventReference musicReference;
    public string scenesManager;
    public Texture2D cursorPencil;
    public Texture2D cursorHand;
    public Texture2D cursorNull;
    [SerializeField] private Animator animatorFade;

    [Header("Letter")]
    public GameObject main;
    public GameObject settings;
    public GameObject sound; 
    public GameObject controls;
    public GameObject fade;
    
    [Header("Button")]
    [SerializeField] private Button buttonMain;
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonControls;
    
    [Header("Slider")]
    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider effects;
    
    ///////////////////////////////////// Events \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Start()
    {
        musicEventInstance = RuntimeManager.CreateInstance(musicReference);
        musicEventInstance.start();
        //musicEventInstance.setParameterByName(musicReference.Path, 0);
        SetupSlider(master, "bus:/Master");
        SetupSlider(music, "bus:/Master/Music");
        SetupSlider(effects, "bus:/Master/SFX");
        
        scenesManager = SceneManager.GetActiveScene().name;
        if (scenesManager == "00 _ MainMenu")
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.SetCursor(cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
            StartCoroutine(StartMainMenu());
        }
    } 
    IEnumerator StartMainMenu() 
    { 
        yield return new WaitForSeconds(1);
        if (scenesManager == "00 _ MainMenu")
        {
            fade.SetActive(false);
            main.GetComponentInChildren<Button>(true).Select();
        }
        fade.SetActive(false);
    }
    
    ///////////////////////////////////// SoundVolume \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void SetupSlider(Slider slider, string busPath)
    {
        RuntimeManager.GetBus(busPath).getVolume(out float _volume);
        slider.value = _volume;
    }
    
    public void SetMasterVolume() { RuntimeManager.GetBus("bus:/Master").setVolume(master.value); }
    public void SetMusicVolume() { RuntimeManager.GetBus("bus:/Master/Music").setVolume(music.value); }
    public void SetSFXVolume() { RuntimeManager.GetBus("bus:/Master/SFX").setVolume(effects.value); }
    
    ///////////////////////////////////// Toggle \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ToggleSettings(bool letterOne)
    {
        if (letterOne == true)
        {
            main.SetActive(false);
            settings.SetActive(true);
            buttonSettings.Select();
        }
        else
        {
            main.SetActive(true);
            buttonMain.Select();
            settings.SetActive(false);
        }
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    public void ToggleSound(bool letterTwo)
    {
        if (letterTwo == true)
        {
            Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.ForceSoftware);
            settings.SetActive(false);
            sound.SetActive(true);
            master.Select();
        }
        else
        {
            Cursor.SetCursor(cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
            settings.SetActive(true);
            buttonSettings.Select();
            sound.SetActive(false);
        }
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    public void ToggleControls(bool letterThree)
    {
        if (letterThree == true)
        {
            settings.SetActive(false);
            controls.SetActive(true);
            buttonControls.Select();
        }
        else
        {
            settings.SetActive(true);
            buttonSettings.Select();
            controls.SetActive(false);
        }
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    
    ///////////////////////////////////// MainMenu \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void StartGame() 
    { 
        fade.SetActive(true); 
        animatorFade.Play("Play");
        StartCoroutine(CStartGame());
    }
    IEnumerator CStartGame()
    {
        yield return new WaitForSeconds(1.6f); 
        SceneManager.LoadScene("Kitchen"); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(cursorNull, Vector2.zero, CursorMode.ForceSoftware);
        musicEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
   
    public void QuitGame()
    {
        fade.SetActive(true);
        animatorFade.Play("Quit");
        StartCoroutine(CQuit());
    }
    IEnumerator CQuit()
    {
        yield return new WaitForSeconds(1.6f); 
        Application.Quit();
    }
    
    ///////////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void MainMenuBack()
    {
        GameManager.instance.TogglePause();
        fade.SetActive(true); 
        animatorFade.Play("FadeOutShort");
        StartCoroutine(CMainMenu());
    }
    IEnumerator CMainMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(cursorNull, Vector2.zero, CursorMode.ForceSoftware);
        musicEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
    
    ///////////////////////////////////// Scenenwechsel  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ScenenManager()
    {
        fade.SetActive(true);
        animatorFade.Play("FadeOut");
        StartCoroutine(CScenenwechsel());
    }
    IEnumerator CScenenwechsel() 
    { 
        yield return new WaitForSeconds(2);
        if (scenesManager == "Kitchen" ) { SceneManager.LoadScene("Psychiatry"); }
        if (scenesManager == "Psychiatry" ) { SceneManager.LoadScene("Save Place"); } 
        if (scenesManager == "SavePlace" ) { SceneManager.LoadScene(""); } 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(cursorNull, Vector2.zero, CursorMode.ForceSoftware);
        musicEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
}


     
  
