using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STOP_MODE = FMOD.Studio.STOP_MODE;



public class Menu : MonoBehaviour
{       ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public Texture2D cursorPencil;
    public Texture2D cursorHand;
    public Texture2D cursorNull;
    public EventReference musicEventReference;
    private EventInstance musicEventInstance;
    public string scenesManager;
        
    [SerializeField] private GameObject fade;
    [SerializeField] private Button firstSelect;
    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;
    public GameObject Main;
    public GameObject Settings;
    public GameObject Sound;
    public GameObject Controls;
        
        
        
        ///////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Awake()
    { 
        master.value = 0.5f; 
        music.value = 0.5f; 
        sfx.value = 0.5f;
    }
    private void Start()
    {
        scenesManager = SceneManager.GetActiveScene().name;
        musicEventInstance = RuntimeManager.CreateInstance(musicEventReference);
        musicEventInstance.start();
        musicEventInstance.setParameterByName(musicEventReference.Path, 2);
        SetupSlider(master, "bus:/Master");
        SetupSlider(music, "bus:/Master/Music");
        SetupSlider(sfx, "bus:/Master/SFX");
    }
        

             ///////////////////////////////////// Slider \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void SetupSlider(Slider slider, string busPath)
    {
        RuntimeManager.GetBus(busPath).getVolume(out float _volume);
        slider.value = _volume;
    }
    public void SetMasterVolume() { RuntimeManager.GetBus("bus:/Master").setVolume(master.value); }
    public void SetMusicVolume() { RuntimeManager.GetBus("bus:/Master/Music").setVolume(music.value); }
    public void SetSFXVolume() { RuntimeManager.GetBus("bus:/Master/SFX").setVolume(sfx.value); }
        
        
        ///////////////////////////////////// Start & Quit Game \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void Scenes()
    {
        fade.SetActive(true);
        StartCoroutine(Fade());
        musicEventInstance.setVolume(0);
    }
    IEnumerator Fade()
    { 
        yield return new WaitForSeconds(2f); 
        if (scenesManager == "MainMenu") 
        { 
            SceneManager.LoadScene("Kitchen");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");  
        }
        musicEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
    public void StopMusic()
    {
        musicEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
        

           ///////////////////////////////////// FadeOut Button Select \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void FirstSelectButton()
    {
        Cursor.SetCursor(cursorPencil, Vector2.zero, CursorMode.ForceSoftware);
        firstSelect.Select();
    }
        
        
    ///////////////////////////////////// Quit Instance \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void Quit() { Application.Quit(); }
}