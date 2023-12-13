using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;





public class InteractableManager : MonoBehaviour
{
    public bool _dishes;
    public bool _rorschach;
    public bool _skillbag;
    public bool _letter;
    public Animator _animator;
    public UnityEvent onInteract;
    
    
    public void OpenKitchen()
    {
        if (_animator == null) { return; }
        _animator.SetTrigger("Open");
    }
    public void CloseKitchen()
    {
        _animator.SetTrigger("Close");
    }
}
