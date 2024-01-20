using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public class PlayerController : MonoBehaviour
{          ///////////////////////////////////// Variablen \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public Texture2D _Cursor;
    [SerializeField] public CinemachineInputProvider _playerCamInputProvider;
    //PlayerInput
    public PlayerInput _playerInput;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction interactAction;
    private InputAction tabAction;
    private InputAction pauseAction;
    
    
    //Move
    private float walkSpeed = 1f;
    private float runSpeed = 3f;
    private float moveSpeed;
    private float minTurnSpeed = 0.2f;
    private float turnSpeed = 5f;
    
    private CharacterController characterController;
    private Transform camTransform;
    
    //Animator
    public Animator _animator;
    
    //Interact
    public InteractableManager _currentInteractable;
        
    
            ///////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    void Start()
    {
        ///////////////////////////////////// Movement \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        _playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
        
        ///////////////////////////////////// Walk & Rund \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        moveAction = _playerInput.actions.FindAction("Move");
        runAction = _playerInput.actions.FindAction("Run");
        
        ///////////////////////////////////// Animations \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        _animator = GetComponentInChildren<Animator>(); //InChildren sucht er alle Unterordner ab
        hallo = _playerInput.actions.FindAction("Hallo");
        cry = _playerInput.actions.FindAction("Cry");
        
        ///////////////////////////////////// Interact \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        interactAction = _playerInput.actions.FindAction("Submit");
        interactAction.performed += Interact;

        ///////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        _playerInput.actions.FindActionMap("UI").FindAction("Pause").performed +=Pause;
        _playerInput.actions.FindActionMap("UI").FindAction("Pause").performed += Pause;
        
        ///////////////////////////////////// QuestLog/Tab \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        tabAction = _playerInput.actions.FindAction("Tab");
        
        ///////////////////////////////////// Cursor \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(_Cursor = null, Vector2.zero, CursorMode.ForceSoftware);
        
        ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        _animator = GetComponentInChildren<Animator>(); //InChildren sucht er alle Unterordner ab
        hallo = _playerInput.actions.FindAction("Hallo");
        cry = _playerInput.actions.FindAction("Cry");
    }
    void Update() 
    {   
        ///////////////////////////////////// Movement \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        Vector2 input = moveAction.ReadValue<Vector2>();
        float horizontalInput = input.x;
        float verticalInput = input.y;

        ///////////////////////////////////// Sprint \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        if (runAction.ReadValue<float>() == 1f)
        { moveSpeed = runSpeed; }
        else
        { moveSpeed = walkSpeed; }
        
        ///////////////////////////////////// Move with Mouse \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(camTransform.right, Vector3.up).normalized * horizontalInput; //rechts,links
        Vector3 verticalVelocity = Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized * verticalInput; //hoch,runter
        Vector3 velocity = Vector3.ClampMagnitude(horizontalVelocity + verticalVelocity, 1); //es wird -2/5, 0-9 = 0-1 berechnet
        
        ///////////////////////////////////// Rotation at Mouse \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        if (velocity.magnitude > minTurnSpeed)
        {
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * turnSpeed);
            transform.rotation = rotation; // DeltaTime passt Frames von unterschiedlichen PC`s an 
        }
        
        ///////////////////////////////////// Gravity \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        characterController.SimpleMove(new Vector3(velocity.x * moveSpeed, 0, velocity.z * moveSpeed));
        
        ///////////////////////////////////// Animations \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        float animatonSpeed = velocity.magnitude;
        if (input == Vector2.zero)
        { animatonSpeed = 0.0f; }
        else if (runAction.inProgress) { }
        _animator.SetFloat("Speed", animatonSpeed);
        _animator.SetBool("Shift", runAction.inProgress);
        
        ///////////////////////////////////// TabUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        /*if (tabAction.inProgress)
        {
            _animatorTabUI.SetBool("Tab", true);
            //ToggleTab();
        }
        else
        {
            _animatorTabUI.SetBool("Tab", false);
            //ToggleTab();
        }*/
    }
    private void OnDisable() //Verhalten Deaktivieren
    {
        interactAction.performed -= Interact;
        _playerInput.actions.FindActionMap("UI").FindAction("Pause").performed -=Pause;
        _playerInput.actions.FindActionMap("UI").FindAction("Pause").performed -= Pause;
        
        ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        //hallo.performed -= HalloEmote;
        //cry.performed -= CryEmote;
    }
    
    
          ///////////////////////////////////// Interact & Collect \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Interact(InputAction.CallbackContext obj)
    {
        if (_currentInteractable == null) { return; }
        _currentInteractable._onInteract.Invoke();
    }
    private void OnTriggerEnter(Collider other) //Collider berühren
    {
        InteractableManager _newInteractable = other.GetComponent<InteractableManager>(); //Achte auf die Klasse ganz oben in deinem Script
        if (_newInteractable == null) { return; }
        _currentInteractable = _newInteractable;
        GameManager.instance.ShowIneractUI(true);
    }
    private void OnTriggerExit(Collider other) //Collider verlassen
    {
        InteractableManager _newInteractable = other.GetComponent<InteractableManager>();

        if (_currentInteractable == null)
        {
            return;
        }
        
        if (_newInteractable == _currentInteractable)
        {
            _currentInteractable = null;
            GameManager.instance.ShowIneractUI(false);
        }
    }

    
    
    /*public RectTransform rectTransform;
    public void ToggleTab()
    {
        if (GameManager.instance._inUI == true) 
        { return; }
        Vector2 currentPositon = rectTransform.anchoredPosition;
        currentPositon.x += 100f / Time.deltaTime;
        rectTransform.anchoredPosition = currentPositon;
    }*/
    
    
        ///////////////////////////////////// PauseUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Pause(InputAction.CallbackContext obj)
    {
        GameManager.instance.TogglePause();
    }

    
        ///////////////////////////////////// Player Inputs \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void DeactivateInput()
    {
       _playerInput.SwitchCurrentActionMap("UI");
       Cursor.lockState = CursorLockMode.Confined;
       _playerCamInputProvider.enabled = false; //Maus
       _currentInteractable = null;
    }
    public void ActivateInput()
    {
        _playerInput.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(_Cursor = null, Vector2.zero, CursorMode.ForceSoftware);
        _playerCamInputProvider.enabled = true; //Maus
    }
    
    
    
    ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private InputAction hallo;
    private InputAction cry;

    private void CryEmote(InputAction.CallbackContext obj)
    {
        DeactivateInput();
        _animator.Play("RigRosie|Hallo");
        //StartCoroutine(ActivatePlayer());
    }
    private void HalloEmote(InputAction.CallbackContext obj)
    {
        DeactivateInput();
        _animator.Play("RigRosie|Hallo");
        //StartCoroutine(ActivatePlayer());
    }
    /*IEnumerator ActivatePlayer()
    {
        yield return new WaitForSeconds(5);
        ActivateInput();
    }*/
}