using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _3_____Scripts.Main
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Slider _master;
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _sfx;
        
        private string _sceneManager;
        private bool _firstStart = false;
        private float _firstVolume = 0.5f;
        
        private void Start()
        {
            if (_firstStart == true)
            { 
                SetupSlider(_master, "bus:/Master");
                SetupSlider(_music, "bus:/Master/Music");
                SetupSlider(_sfx, "bus:/Master/SFX");
            }
            else
            {
                SetupSlider(_master, float _firstVolume);
                SetupSlider(_music, "bus:/Master/Music");
                SetupSlider(_sfx, "bus:/Master/SFX");
            }
            
        }


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
    }
}