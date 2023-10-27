using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minTurnSpeed = 1f;
    public float turnSpeed = 5f;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _interactAction;
    private Rigidbody _rigidbody;
    private Transform _camTransform;
    private Animator _animator;
    public Interactable _currentInteractable;


    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _interactAction = _playerInput.actions.FindAction("Interact");
        _rigidbody = GetComponent<Rigidbody>();
        _camTransform = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>(); //InChildren sucht er alle Unterordner ab

        _interactAction.performed += Interact;
            
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void OnDisable()
    {
        _interactAction.performed -= Interact;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();
        float horizontalInput = input.x;
        float verticalInput = input.y;

        Vector3 horizontalVelocity =
            Vector3.ProjectOnPlane(_camTransform.right, Vector3.up).normalized * horizontalInput;
        Vector3 verticalVelocity = Vector3.ProjectOnPlane(_camTransform.forward, Vector3.up).normalized * verticalInput;
        Vector3 velocity =
            Vector3.ClampMagnitude(horizontalVelocity + verticalVelocity, 1); //es wird -2/5, 0-9 = 0-1 berechnet

        // Rotate
        if (velocity.magnitude > minTurnSpeed)
        {
            Quaternion rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.LookRotation(velocity),
                Time.deltaTime * turnSpeed); // DeltaTime passt Frames von unterschiedlichen PC`s an
            _rigidbody.rotation = rotation;
        }

        

        _rigidbody.velocity =
            velocity = new Vector3(velocity.x * moveSpeed, _rigidbody.velocity.y, velocity.z * moveSpeed);

        //Animation
        float animatonSpeed = velocity.magnitude;
        if (input == Vector2.zero)
        {
            animatonSpeed = 0.0f;
        }

        _animator.SetFloat("Speed", animatonSpeed);


    }

    void Interact(InputAction.CallbackContext obj)
    {
        if (_currentInteractable == null)
        {
            return;
        }
        
        Debug.Log("interact");
        
        _currentInteractable.onInteract.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {

        Interactable
            _newInteractable = other.GetComponent<Interactable>(); //Achte auf die Klasse ganz oben in deinem Script

        if (_newInteractable == null)
        {
            return;
        }

        _currentInteractable = _newInteractable;
        GameManager.instance.ShowInteractUI(true);

    }

    private void OnTriggerExit(Collider other)
    {
        Interactable _newInteractable = other.GetComponent<Interactable>();

        if (_currentInteractable == null)
        {
            return;
        }
        
        if (_newInteractable == _currentInteractable)
        {
            _currentInteractable = null;
            GameManager.instance.ShowInteractUI(false);
        }
        
        
    }
}
