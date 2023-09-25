using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Rigidbody _rigidbody;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();
        float horizontalInput = input.x;
        float verticalInput = input.y;

        _rigidbody.velocity = new Vector3(horizontalInput * moveSpeed, y:_rigidbody.velocity.y, verticalInput * moveSpeed);
        
        

    }
}
