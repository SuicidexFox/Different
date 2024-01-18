using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;





public class InteractableManager : MonoBehaviour
{
    public UnityEvent _onInteract;
    //public GameObject _billboard;
    
    
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null) {return;}
        _billboard.SetActive(true);
    }
    private void OnTriggerExit(Collider other) //Collider verlassen
    {
        if (other.GetComponent<PlayerController>() == null) {return;}
        _billboard.SetActive(false);
    }*/
}
