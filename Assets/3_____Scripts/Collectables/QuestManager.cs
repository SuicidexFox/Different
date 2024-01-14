using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

public class QuestManager : MonoBehaviour
    {
        [Header("ShowQuest")] 
        [SerializeField] private GameObject questUI;
        [SerializeField] private Button button;
        public Animator _animator;
        public CinemachineVirtualCamera _questCam;
        
        //Player
        public PlayerController _player;
        
        
        //Canvas
        public void ShowQuestUI()
        { 
            _player.DeactivateInput();
            questUI.SetActive(true);
            if (questUI != null)
            {
             _questCam.Priority = 11;   
            }
            
            GameManager.instance.ShowIneractUI(false);
            StartCoroutine(FocusButton());
            GameManager.instance._inUI = true;
        }
        IEnumerator FocusButton()
        {
            yield return new WaitForEndOfFrame();
            button.Select();   
        }
        public void Animation()
        { _animator.Play("QuestUIScaleSmal");}
        public void CloseQuestUI()
        {
            _player.ActivateInput();
            if (questUI != null)
            {
                _questCam.Priority = 0;   
            }
            Destroy(gameObject);
            GameManager.instance._inUI = false;
        }
    }