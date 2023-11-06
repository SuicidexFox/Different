using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //PlayerInput
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _interactAction;
    
    //Move
    public float _moveSpeed = 2f;
    public float _minTurnSpeed = 1f;
    public float _turnSpeed = 5f;
    public float _runSpeed = 3f;
    private Rigidbody _rigidbody;
    private Transform _camTransform;
    
    //Animator
    private Animator _animator;
    
    //Interact
    public Interactable _currentInteractable;
        
    
    void Start() 
    {   //Walk
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _camTransform = Camera.main.transform;
        
        
        //Walk & Run
        _moveAction = _playerInput.actions.FindAction("Move");
        _runAction = _playerInput.actions.FindAction("Run");
        
        //Animator
        _animator = GetComponentInChildren<Animator>(); //InChildren sucht er alle Unterordner ab
        
        //Interact
        _interactAction = _playerInput.actions.FindAction("Interact");
        _interactAction.performed += Interact;
        
        //MausCursor deaktivieren
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() 
    {   //Bewegung
        //Move with Mouse
        Vector2 input = _moveAction.ReadValue<Vector2>();
        float horizontalInput = input.x;
        float verticalInput = input.y;

        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_camTransform.right, Vector3.up).normalized * horizontalInput; //rechts,links
        Vector3 verticalVelocity = Vector3.ProjectOnPlane(_camTransform.forward, Vector3.up).normalized * verticalInput; //hoch,runter
        Vector3 velocity = Vector3.ClampMagnitude(horizontalVelocity + verticalVelocity, 1); //es wird -2/5, 0-9 = 0-1 berechnet
        
        
        // Rotation at Mouse
        if (velocity.magnitude > _minTurnSpeed)
        {
            Quaternion rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * _turnSpeed);
            _rigidbody.rotation = rotation; // DeltaTime passt Frames von unterschiedlichen PC`s an 
        }
        // Gravity
        _rigidbody.velocity = velocity = new Vector3(velocity.x * _moveSpeed, _rigidbody.velocity.y, velocity.z * _moveSpeed);
             
        
        //Animator
        float animatonSpeed = velocity.magnitude;
        if (input == Vector2.zero)
        {
            animatonSpeed = 0.0f;
        }
        _animator.SetFloat("Speed", animatonSpeed);
    }
    
    
    private void OnDisable() //Verhalten Deaktivieren
    {
        _interactAction.performed -= Interact;
    }
    

    private void Interact(InputAction.CallbackContext obj)
    {
        if (_currentInteractable == null)
        {
            return;
        }
        _currentInteractable.onInteract.Invoke();
        GameManager.instance.ShowDialogUI(true);
        
    }
    private void OnTriggerEnter(Collider other) //Collider berühren
    {
        Interactable _newInteractable = other.GetComponent<Interactable>(); //Achte auf die Klasse ganz oben in deinem Script

        if (_newInteractable == null)
        {
            return;
        }
        _currentInteractable = _newInteractable;
        GameManager.instance.ShowIneractUI(true);
    }
    private void OnTriggerExit(Collider other) //Collider verlassen
    {
        Interactable _newInteractable = other.GetComponent<Interactable>();

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
}