using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void Kitchen()
        {
            SceneManager.LoadScene("Kitchen");
        }
    }
}