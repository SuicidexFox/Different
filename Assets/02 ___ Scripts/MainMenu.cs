using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MainMenu : MonoBehaviour
{   ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public static MainMenu instance;
    public EventInstance musicInstance;
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
    public GameObject tutorial;
    public Button tutorialButton;
    public GameObject fadePsychiatry;
    public CinemachineVirtualCamera fadeOutCam;
    
    [Header("Button")]
    public Button buttonMain;
    public Button buttonSettings;
    public Button buttonControls;
    
    [Header("Slider")]
    public Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider effects;
    
    ///////////////////////////////////// Events \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Start()  //Startet das Main Menu, nicht die Pause
    {
        musicInstance = RuntimeManager.CreateInstance(musicReference);
        musicInstance.setParameterByName("MusicStage", 0);
        musicInstance.start();
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
    { yield return new WaitForSeconds(1); { fade.SetActive(false); buttonMain.Select(); }}
    
    private void Update()
    {
        if (scenesManager == "00 _ MainMenu")
        { return; }
        else
        {
            if (GameManager.instance.inUI == true) { musicInstance.setParameterByName("MusicStage", 2); }
            else { musicInstance.setParameterByName("MusicStage", 0); } 
        }
        
    }
    

    ///////////////////////////////////// SoundVolume \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void SetupSlider(Slider slider, string busPath) { RuntimeManager.GetBus(busPath).getVolume(out float _volume); slider.value = _volume; }
    
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
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSidePause");
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
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSidePause");
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
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSidePause");
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
        musicInstance.stop(STOP_MODE.IMMEDIATE);
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
    public void Continue() { GameManager.instance.TogglePause(); }
    public void MainMenuBack()
    {
        Time.timeScale = 1.0f;
        fade.SetActive(true); 
        animatorFade.Play("FadeOut");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(cursorNull, Vector2.zero, CursorMode.ForceSoftware);
        StartCoroutine(CMainMenu());
    }
    IEnumerator CMainMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("00 _ MainMenu");
        musicInstance.stop(STOP_MODE.IMMEDIATE);
    }
    
    ///////////////////////////////////// Scenenwechsel  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ScenenManager()
    {
        fade.SetActive(true);
        animatorFade.Play("FadeOut");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(cursorNull, Vector2.zero, CursorMode.ForceSoftware);
        if (scenesManager == "Kitchen")
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE);
            RuntimeManager.PlayOneShot("event:/SFX/Rosie/Voice/Schrei");
            StartCoroutine(CKitchen());
        }
        if (scenesManager == "Psychiatry") { fade.SetActive(true); StartCoroutine(CPsychiatry()); }
        if (scenesManager == "SavePlace") 
        { 
            fadeOutCam.gameObject.SetActive(true);
            GameManager.instance.playerController.cinemachineBrain.m_DefaultBlend.m_Time = 4;
            StartCoroutine(CFadeOutCam()); 
        }
    }

    IEnumerator CKitchen()
    {
        yield return new WaitForSeconds(7);
        StartCoroutine(CScenenwechsel());
    }
    IEnumerator CPsychiatry()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(CScenenwechsel());
    }
    IEnumerator CFadeOutCam() { yield return new WaitForSeconds(3); fade.SetActive(true); StartCoroutine(CSavePlace()); }
    IEnumerator CSavePlace(){ yield return new WaitForSeconds(5); StartCoroutine(CScenenwechsel()); }
    
    IEnumerator CScenenwechsel() 
    { 
        yield return new WaitForSeconds(2);
        if (scenesManager == "Kitchen" ) { SceneManager.LoadScene("Psychiatry"); }
        if (scenesManager == "Psychiatry" ) { SceneManager.LoadScene("Sequence"); } 
        if (scenesManager == "SavePlace" ) { SceneManager.LoadScene("Credits"); } 
        musicInstance.setVolume(0);
        musicInstance.stop(STOP_MODE.IMMEDIATE);
    }
}