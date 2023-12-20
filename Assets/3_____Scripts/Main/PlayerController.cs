using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public CinemachineInputProvider _playerCamInputProvider;
    //PlayerInput
    public PlayerInput _playerInput;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction interactAction;
    private InputAction tabAction;
    private InputAction hallo;
    private InputAction cry;
    
    
    //Move
    private float walkSpeed = 2f;
    private float runSpeed = 3f;
    private float moveSpeed;
    private float minTurnSpeed = 0.2f;
    private float turnSpeed = 5f;
    private CharacterController characterController;
    private Transform camTransform;
    
    //Animator
    public Animator _animator;
    public GameObject _tabUI;
    public Animator _animatorTabUI;
    
    //Interact
    public InteractableManager _currentInteractable;
        
    
    
    
    //Events
    void Start()
    {   //Walk
        _playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
        
        
        //Walk & Run
        moveAction = _playerInput.actions.FindAction("Move");
        runAction = _playerInput.actions.FindAction("Run");
        
        //Animator
        _animator = GetComponentInChildren<Animator>(); //InChildren sucht er alle Unterordner ab
        hallo = _playerInput.actions.FindAction("Hallo");
        cry = _playerInput.actions.FindAction("Cry");
        
        //Interact
        interactAction = _playerInput.actions.FindAction("Interact");
        interactAction.performed += Interact;
        
        //MausCursor deaktivieren
        Cursor.lockState = CursorLockMode.Locked;
        
        //QuestLog
        tabAction = _playerInput.actions.FindAction("Tab");
    }
    void Update() 
    {   
        //Bewegung
        Vector2 input = moveAction.ReadValue<Vector2>();
        float horizontalInput = input.x;
        float verticalInput = input.y;

        //Sprint
        if (runAction.ReadValue<float>() == 1f)
        { moveSpeed = runSpeed; }
        else
        { moveSpeed = walkSpeed; }
        
        //Move with Mouse
        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(camTransform.right, Vector3.up).normalized * horizontalInput; //rechts,links
        Vector3 verticalVelocity = Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized * verticalInput; //hoch,runter
        Vector3 velocity = Vector3.ClampMagnitude(horizontalVelocity + verticalVelocity, 1); //es wird -2/5, 0-9 = 0-1 berechnet
        
        //Rotation at Mouse
        if (velocity.magnitude > minTurnSpeed)
        {
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * turnSpeed);
            transform.rotation = rotation; // DeltaTime passt Frames von unterschiedlichen PC`s an 
        }
        
        // Gravity
        characterController.SimpleMove(new Vector3(velocity.x * moveSpeed, 0, velocity.z * moveSpeed));
        
        //Animator
        float animatonSpeed = velocity.magnitude;
        
        if (input == Vector2.zero)
        { animatonSpeed = 0.0f; }
        else if (runAction.inProgress)
        { }
        _animator.SetFloat("Speed", animatonSpeed);
        _animator.SetBool("Shift", runAction.inProgress);
        if (hallo.inProgress) { _animator.Play("RigRosie|Hallo"); }
        if (cry.inProgress) { _animator.Play("RigRosie|Cry"); }
        
        //Tab
        if (tabAction.inProgress)
        { _animatorTabUI.SetBool("Tab", true); }
        else
        { _animatorTabUI.SetBool("Tab", false); }
    }
    private void OnDisable() //Verhalten Deaktivieren
    {
        interactAction.performed -= Interact;
    }
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


    //Inputs
    public void DeactivateInput()
    {
       _playerInput.SwitchCurrentActionMap("UI"); 
       _playerCamInputProvider.enabled = false; //Maus
       _currentInteractable = null;
    }
    public void ActivateInput()
    {
        _playerInput.SwitchCurrentActionMap("Player"); 
        _playerCamInputProvider.enabled = true; //Maus
    }
    
    
    

    
    
    //public void AnimationHello() 
        //Player
        //_playerInput.SwitchCurrentActionMap("UI");
        //Cursor.lockState = CursorLockMode.Confined; //Maus
        //_animator.Play("RigRosie|Hallo");
    
    //public void AnimationHelloClose() 
        //_playerInput.SwitchCurrentActionMap("Player");
        //Cursor.lockState = CursorLockMode.Locked; //Maus
    
    
}