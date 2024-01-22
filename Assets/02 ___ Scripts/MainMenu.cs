using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
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
    
    [Header("Slider")]
    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider effects;
    
    
    
    ///////////////////////////////////// Events \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Start()
    {
        Cursor.SetCursor(cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.Confined;
        scenesManager = SceneManager.GetActiveScene().name;
        musicEventInstance = RuntimeManager.CreateInstance(musicReference);
        if (musicReference.Path != null)
        { 
          musicEventInstance.start();
          musicEventInstance.setParameterByName(musicReference.Path, 0);
        }
        SetupSlider(master, "bus:/Master");
        SetupSlider(music, "bus:/Master/Music");
        SetupSlider(effects, "bus:/Master/SFX");
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
    
    
    //////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void SelectButton()
    {
        fade.SetActive(false);
        if (scenesManager == "Kitchen") { SelectButtonKitchen(); animatorFade.Play("FadeOutKitchen"); return; }
        GetComponentInChildren<Button>().Select();
    }
    
    ///////////////////////////////////// Toggle \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void ToggleSettings(bool letterOne)
    {
        if (letterOne == true)
        {
            main.SetActive(false);
            settings.SetActive(true);
        }
        else
        {
            main.SetActive(true);
            settings.SetActive(false);
        }
        GetComponentInChildren<Button>(true).Select();
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    public void ToggleSound(bool letterTwo)
    {
        if (letterTwo == true)
        {
            Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.ForceSoftware);
            settings.SetActive(false);
            sound.SetActive(true);
        }
        else
        {
            Cursor.SetCursor(cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
            settings.SetActive(true);
            sound.SetActive(false);
        }
        GetComponentInChildren<Button>(true).Select();
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    public void ToggleControls(bool letterThree)
    {
        if (letterThree == true)
        {
            settings.SetActive(false);
            controls.SetActive(true);
        }
        else
        {
            settings.SetActive(true);
            controls.SetActive(false);
        }
        GetComponentInChildren<Button>(true).Select();
        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    
    
    ///////////////////////////////////// MainMenu \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    /////////////////////////////////////   Start  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationEventStart() { fade.SetActive(true); animatorFade.Play("Play"); }
    public void StartGame() { SceneManager.LoadScene("Kitchen"); musicEventInstance.setVolume(0); }
    
    ///////////////////////////////////// Quit \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationEventQuit() { fade.SetActive(true); animatorFade.Play("Quit"); }
    public void Quit() { Application.Quit(); }
    
    
    
    ///////////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    ///////////////////////////////////// Back MainMenu  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationEventMainMenu() { fade.SetActive(true); animatorFade.Play("FadeOut"); }
    public void BackMainMenu() { SceneManager.LoadScene("MainMenu"); }

    ///////////////////////////////////// Scenenwechsel  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationEventScenenwechsel() { fade.SetActive(true); animatorFade.Play("FadeOutShort"); }
    public void Scenenwechsel()
    {
        if (scenesManager == "Kitchen") { SceneManager.LoadScene("Psychiatry"); }
        if (scenesManager == "Psychiatry") { SceneManager.LoadScene("Save Place"); }
    }
    
    ///////////////////////////////////// Kitchen  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationEventLetter() { fade.SetActive(true); animatorFade.Play("FadeOutKitchen"); }
    public void SelectButtonKitchen() { fade.GetComponentInChildren<Button>().Select();}
}


     
  
