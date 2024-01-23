using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator animator;
    public void StartDialog()
    {
        animator.Play("Interact");
    }
    public void Close()
    {
        animator.Play("Close");
    }
    
}
