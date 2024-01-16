using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _3_____Scripts.Main
{
    public class MenuManager : MonoBehaviour
    {
        public EventInstance musicEventInstance;
        [SerializeField] private  Texture2D _cursor;
        [SerializeField] private Button _firstSelect;
        [SerializeField] private Slider _master;
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _sfx;
        
        public GameObject fade;
        
        private bool _firstStart = false;
        private string _scenesManager;

        
        private void Start()
        {
            _scenesManager = SceneManager.GetActiveScene().name;
            
            SetupSlider(_master, "bus:/Master");
            SetupSlider(_music, "bus:/Master/Music");
            SetupSlider(_sfx, "bus:/Master/SFX");
        }
        

        //Slider
        private void SetupSlider(Slider slider, string busPath)
        {
            RuntimeManager.GetBus(busPath).getVolume(out float _volume);
            slider.value = _volume;
        }
        public void SetMasterVolume()
        {
            RuntimeManager.GetBus("bus:/Master").setVolume(_master.value);
        }
        public void SetMusicVolume()
        {
            RuntimeManager.GetBus("bus:/Master/Music").setVolume(_music.value);
        }
        public void SetSFXVolume()
        {
            RuntimeManager.GetBus("bus:/Master/SFX").setVolume(_sfx.value);
            RuntimeManager.PlayOneShot("event:/SFX/UI/ButtonHover");
        }
        
        
        
       //Scenes
        public void Scenes()
        {
            fade.SetActive(true);
            StartCoroutine(Fade());
        }
        IEnumerator Fade()
        {
            yield return new WaitForSeconds(5f);
            if (_scenesManager == "MainMenu")
            {
                SceneManager.LoadScene("Kitchen");
            }
            else
            {
                SceneManager.LoadScene("MainMenu");  
            }

            musicEventInstance.setVolume(0f);
        }



        //Animations
        public void FirstSelectButton()
        {
            _firstSelect.Select();
            Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}