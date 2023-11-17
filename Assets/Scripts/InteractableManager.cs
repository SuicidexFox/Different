using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableManager : MonoBehaviour
{
    public GameObject _collectable;
    public Animator _animator;
    public UnityEvent onInteract;

    public void DestroyCollect()
    {
        _collectable.SetActive(false);
    }
}
