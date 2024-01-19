using System;
using UnityEngine;
using UnityEngine.UI;

namespace _3_____Scripts.Collectables
{
    public class Billboard : MonoBehaviour
    {
        private Camera _cam;
        public SpriteRenderer _sprite;
        public bool _NPC;

        private void Start()
        { 
            _cam = Camera.main;
            if (_NPC = true)
            {
                return;
            }
            _sprite.enabled = false;
        }

        private void LateUpdate()
        {
            transform.rotation = _cam.transform.rotation;
        }
        
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() == null) {return;}
            _sprite.enabled = true;
        }
        private void OnTriggerExit(Collider other) //Collider verlassenf
        {
            if (other.GetComponent<PlayerController>() == null) {return;}
            _sprite.enabled = false;
        }
    }
}