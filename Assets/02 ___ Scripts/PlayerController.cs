using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{          ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public Interactable currentInteractable;
    public CinemachineInputProvider playerCamInputProvider;
    
    public PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction interactAction;
    private InputAction tabAction;
    private InputAction pauseAction;
    
    private CharacterController characterController;
    private Transform camTransform;
    public Animator animator;
    
    //Move
    private float walkSpeed = 1f;
    private float runSpeed = 3f;
    private float moveSpeed;
    private float minTurnSpeed = 0.2f;
    private float turnSpeed = 5f;
    
    
    ///////////////////////////////////// Start \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void Start()
    {
        ///////////////////////////////////// Movement \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;
        
        ///////////////////////////////////// Walk & Run \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        moveAction = playerInput.actions.FindAction("Move");
        runAction = playerInput.actions.FindAction("Run");
        
        
        ///////////////////////////////////// Interact \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        interactAction = playerInput.actions.FindAction("Submit");
        interactAction.performed += Interact;

        ///////////////////////////////////// Pause \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        playerInput.actions.FindActionMap("Player").FindAction("Pause").performed +=Pause;
        playerInput.actions.FindActionMap("UI").FindAction("Pause").performed += Pause;
        
        ///////////////////////////////////// QuestLog/Tab \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        tabAction = playerInput.actions.FindAction("Tab");
        
        ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        animator = GetComponentInChildren<Animator>();
        hallo = playerInput.actions.FindAction("Hallo");
        hallo.performed += HalloEmote;
        cry = playerInput.actions.FindAction("Cry");
        cry.performed += CryEmote;
    }
    public void Update() 
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
        animator.SetFloat("Speed", animatonSpeed);
        animator.SetBool("Shift", runAction.inProgress);
        
        ///////////////////////////////////// TabUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        if (tabAction.inProgress)
        {
            //GameManager.instance.ToggleEmote();
        }
        else
        {
            //GameManager.instance.ToggleEmote();
        }
    }
    private void OnDisable() //Disable behavior 
    {
        interactAction.performed -= Interact;
        playerInput.actions.FindActionMap("Player").FindAction("Pause").performed -=Pause;
        playerInput.actions.FindActionMap("UI").FindAction("Pause").performed -= Pause;
        
        ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        hallo.performed -= HalloEmote;
        cry.performed -= CryEmote;
    }
    
    
    ///////////////////////////////////// Interact & Collect \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Interact(InputAction.CallbackContext obj)
    {
        if (currentInteractable == null) { return; }
        currentInteractable._onInteract.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        Interactable _newInteractable = other.GetComponent<Interactable>();
        if (_newInteractable == null) { return; }
        currentInteractable = _newInteractable;
        GameManager.instance.ShowIneractUI(true);
    }
    private void OnTriggerExit(Collider other)
    { 
        Interactable _newInteractable = other.GetComponent<Interactable>();
        if (currentInteractable == null) { return; } 
        if (_newInteractable == currentInteractable)
        {
            currentInteractable = null;
            GameManager.instance.ShowIneractUI(false);
        }
    }
    
    
    ///////////////////////////////////// PauseUI \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private void Pause(InputAction.CallbackContext obj) { GameManager.instance.TogglePause(); }

    
    ///////////////////////////////////// Player Inputs \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void DeactivateInput()
    {
       playerInput.SwitchCurrentActionMap("UI");
       Cursor.lockState = CursorLockMode.Confined;
       playerCamInputProvider.enabled = false;
       currentInteractable = null;
    }
    public void ActivateInput()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        playerCamInputProvider.enabled = true;
    }
    
    
    ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private InputAction hallo;
    private InputAction cry;
    private void CryEmote(InputAction.CallbackContext obj)
    {
        animator.Play("Cry");
        GameManager.instance.ToggleEmote();
    }
    private void HalloEmote(InputAction.CallbackContext obj)
    {
        animator.Play("Hallo");
        GameManager.instance.ToggleEmote();
    }
}
