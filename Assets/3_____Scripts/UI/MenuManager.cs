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
using Vector3 = UnityEngine.Vector3;

namespace _3_____Scripts.Main
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] public EventInstance musicEventInstance;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject _object;
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
            _object.SetActive(true);
            //Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.ForceSoftware);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}