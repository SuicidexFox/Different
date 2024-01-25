using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{          ///////////////////////////////////// Variable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public Interactable currentInteractable;
    public CinemachineInputProvider playerCamInputProvider;
    public string currentControlScheme;
    
    public PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction interactAction;
    public bool questLog = false;
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
    

    // Funktion, um das aktuelle Control Scheme abzufragen
    public string GetCurrentControlScheme()
    { return currentControlScheme; }
    
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
        
        
        ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        hallo = playerInput.actions.FindAction("EmoteHallo");
        hallo.performed += HalloEmote;
        cry = playerInput.actions.FindAction("EmoteCry");
        cry.performed += CryEmote;
    }
    public void Update() 
    {   ////////////////////////////////// Movement \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
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
        
        ///////////////////////////////////// QuestLog \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        //Keyboard
        if (Input.GetKeyDown(KeyCode.Tab))
        { questLog = true; }
        // Prüfe, ob die Taste losgelassen wurde
        if (Input.GetKeyUp(KeyCode.Tab))
        { questLog = false; GameManager.instance.questLog.ResetCanvasPosition(); }
        // Bewege das Canvas, wenn die Taste gedrückt wird
        if (questLog)
        { GameManager.instance.questLog.MoveCanvas();}
        //Controller
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.dpad.right.wasPressedThisFrame) { questLog = true; }
            if (gamepad.dpad.right.wasReleasedThisFrame)
            { questLog = false; GameManager.instance.questLog.ResetCanvasPosition(); }
            if (questLog) { GameManager.instance.questLog.MoveCanvas(); }
        }
    }   
    
    private void OnDisable() //Disable behavior 
    {
        interactAction.performed -=Interact;
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
       playerCamInputProvider.enabled = false;
       currentInteractable = null;
       Cursor.lockState = CursorLockMode.Confined;
    }
    public void ActivateInput()
    {
        playerInput.SwitchCurrentActionMap("Player");
        playerCamInputProvider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(GameManager.instance.menu.cursorNull, Vector2.zero, CursorMode.ForceSoftware);
    }
    
    
    ///////////////////////////////////// Extras \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    private InputAction hallo;
    private InputAction cry;
    private void CryEmote(InputAction.CallbackContext obj)
    {
        animator.Play("Cry");
        RuntimeManager.PlayOneShot("event:/SFX/Rosie/Emotes/Cry");
        Emote();
    }
    private void HalloEmote(InputAction.CallbackContext obj)
    {
        animator.Play("Hallo");
        RuntimeManager.PlayOneShot("event:/SFX/Rosie/Emotes/Hallo");
        Emote();
    }
    public void Emote()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }
}
