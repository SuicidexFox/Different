using System;
using UnityEngine;

namespace _3_____Scripts.Collectables
{
    public class Billboard : MonoBehaviour
    {
        private Camera _cam;

        private void Start()
        { 
            _cam = Camera.main;
        }

        private void LateUpdate()
        {
            transform.rotation = _cam.transform.rotation;
        }
    }
}