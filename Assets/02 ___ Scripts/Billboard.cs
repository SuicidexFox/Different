using System;
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
        if (npc == true)
        {
            return;
        }
        sprite.enabled = false;
    }

    private void Update()
    {
        if (npc == true)
        {
            if (GameManager.instance.inUI == true)
            {
                sprite.enabled = false;
            }
            else
            {
                sprite.enabled = true;
            }
        }
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
