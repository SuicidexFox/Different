using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using Vector3 = UnityEngine.Vector3;

namespace _3_____Scripts.Main
{
    public class Menu : MonoBehaviour
    {       ///////////////////////////////////// Variablen \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public EventReference musicEventReference;
        private EventInstance _musicEventInstance;
        public string _scenesManager;
        
        [SerializeField] private GameObject _fade;
        [SerializeField] private Button _firstSelect;
        [SerializeField] private Slider _master;
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _sfx;
        public Texture2D _cursor;
        
        
             ///////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        private void Awake()
        {
            _master.value = 0.5f;
            _music.value = 0.5f; 
            _sfx.value = 0.5f;
        }
        private void Start()
        {
            _scenesManager = SceneManager.GetActiveScene().name;
            _musicEventInstance = RuntimeManager.CreateInstance(musicEventReference);
            _musicEventInstance.start();
            _musicEventInstance.setParameterByName(musicEventReference.Path, 2);
            SetupSlider(_master, "bus:/Master");
            SetupSlider(_music, "bus:/Master/Music");
            SetupSlider(_sfx, "bus:/Master/SFX");
        }
        

             ///////////////////////////////////// Slider \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        private void SetupSlider(Slider slider, string busPath)
        {
            RuntimeManager.GetBus(busPath).getVolume(out float _volume);
            slider.value = _volume;
        }
        public void SetMasterVolume() { RuntimeManager.GetBus("bus:/Master").setVolume(_master.value); }
        public void SetMusicVolume() { RuntimeManager.GetBus("bus:/Master/Music").setVolume(_music.value); }
        public void SetSFXVolume() { RuntimeManager.GetBus("bus:/Master/SFX").setVolume(_sfx.value); }
        
        
        ///////////////////////////////////// Start & Quit Game \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public void Scenes()
        {
            _fade.SetActive(true);
            StartCoroutine(Fade());
            _musicEventInstance.setVolume(0);
        }
        IEnumerator Fade()
        {
            yield return new WaitForSeconds(2f);
            if (_scenesManager == "MainMenu")
            {
                SceneManager.LoadScene("Kitchen");
            }
            else
            {
                SceneManager.LoadScene("MainMenu");  
            }
            _musicEventInstance.stop(STOP_MODE.IMMEDIATE);
        }
        public void StopMusic()
        {
            _musicEventInstance.stop(STOP_MODE.IMMEDIATE);
        }
        

           ///////////////////////////////////// FadeOut Button Select \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public void FirstSelectButton()
        {
            _firstSelect.Select();
            //Cursor.SetCursor(_cursor = null, Vector2.zero, CursorMode.ForceSoftware);
        }
        
        
             ///////////////////////////////////// Quit Instance \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public void Quit()
        {
            Application.Quit();
        }
    }
}