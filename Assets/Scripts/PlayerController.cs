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
    private Rigidbody _rigidbody;
    private Transform _camTransform;
    private Animator _animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _rigidbody = GetComponent<Rigidbody>();
        _camTransform = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>(); //InChildren sucht er alle Unterordner ab

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();
        float horizontalInput = input.x;
        float verticalInput = input.y;

        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_camTransform.right, Vector3.up).normalized * horizontalInput;
        Vector3 verticalVelocity = Vector3.ProjectOnPlane(_camTransform.forward, Vector3.up).normalized * verticalInput;
        Vector3 velocity = Vector3.ClampMagnitude(horizontalVelocity + verticalVelocity, 1) * moveSpeed;
        
        // Rotate
        if (velocity.magnitude > minTurnSpeed)
        {
            Quaternion rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.LookRotation(velocity),
                Time.deltaTime * turnSpeed); // DeltaTime passt Frames von unterschiedlichen PC`s an
            _rigidbody.rotation = rotation;
        }

        // Gravitation
        velocity.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = velocity;
        
        
        //Animation
        _animator.SetFloat("Speed", Mathf.Clamp(velocity.magnitude, 0, moveSpeed));
        
       
    }
}
