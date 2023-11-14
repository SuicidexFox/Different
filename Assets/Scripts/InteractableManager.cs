using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;

public class InteractableManager : MonoBehaviour
{
    public GameObject _collectable;
    public Animator _animator;
    public UnityEvent onInteract;
}
