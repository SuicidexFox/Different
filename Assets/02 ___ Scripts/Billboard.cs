using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera cam;
    public SpriteRenderer sprite;
    public bool npc;

    private void Start()
    { 
        cam = Camera.main;
        if (npc = true)
        {
            return;
        }
        sprite.enabled = false;
    }
    private void LateUpdate()
    {
        transform.rotation = cam.transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null) {return;}
        sprite.enabled = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null) {return;}
        sprite.enabled = false;
    }
}
