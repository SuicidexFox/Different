using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private EventInstance musicEventInstance;
    [SerializeField] private EventReference musicReference;
    [SerializeField] private Texture2D cursorPencil;
    [SerializeField] private Texture2D cursorHand;
    [SerializeField] private Animator animatorFade;

    [Header("Letter")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject sound; 
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject fade;

    [Header("Button")]
    [SerializeField] private Button firstSelect;
    
    [Header("Slider")]
    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider effects;
    
    
    
    ///////////////////////////////////// Events \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Awake()
    {
        master.value = 0.5f;
        music.value = 0.5f;
        effects.value = 0.5f;
    }
    private void Start()
    {
        Cursor.SetCursor(cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
        musicEventInstance = RuntimeManager.CreateInstance(musicReference);
        musicEventInstance.start();
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
    public void SelectButton() { firstSelect.Select(); }
    
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

        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    public void Controls(bool letterThree)
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

        RuntimeManager.PlayOneShot("event:/SFX/UI_UX/Menu/Open_NextSide");
    }
    
    ///////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationEventStart()
    {
        fade.SetActive(true);
        animatorFade.Play("aKitchen");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Kitchen");
        musicEventInstance.setVolume(0);
    }
    
    ///////////////////////////////////// Quit \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void AnimationEventQuit()
    { animatorFade.Play("Quit"); }
    public void Quit() { Application.Quit(); }
}

     
  
