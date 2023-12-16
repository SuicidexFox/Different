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
        private PlayerController player;
        
        
        //Canvas
        public void ShowQuestUI()
        { 
            player._playerInput.SwitchCurrentActionMap("UI");
            player._playerCamInputProvider.enabled = false;
            questUI.SetActive(true);
            _questCam.Priority = 11;
            GameManager.instance.ShowIneractUI(false);
            player.DeactivateInput();
            
            
            StartCoroutine(FocusButton());
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
            player.ActivateInput();
            _questCam.Priority = 0;
            Destroy(this);
        }
    }