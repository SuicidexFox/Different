using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sequence : MonoBehaviour
{
    public string scenenmanager;
    public Animator fadeAnimator;
    public Animator rosie;
    public Animator innerChilde;
    public Button quitGameButton;
    public Texture2D cursor;


    private void Start()
    {
        scenenmanager = SceneManager.GetActiveScene().name;
        if (scenenmanager == "Credits") 
        { StartCoroutine(StartCredits()); return; }
        StartCoroutine(StartAnimation());
        GetComponent<Animator>().enabled = true;
    }
    IEnumerator StartAnimation() { yield return new WaitForSeconds(2); fadeAnimator.enabled = false; }

    

    ///////////////////////////////////// Sequence \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void Scenenwechsel()
    {
        fadeAnimator.enabled = true;
        fadeAnimator.Play("FadeOut");
        StartCoroutine(CScenenwechsel());
    }
    IEnumerator CScenenwechsel() { yield return new WaitForSeconds(2); SceneManager.LoadScene("SavePlace"); }
    public void Rosie() { rosie.Play("Sequence"); }
    public void InnerChilde() {innerChilde.Play("Hallo");}
    
    ///////////////////////////////////// Credits \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    IEnumerator StartCredits()
    { yield return new WaitForSeconds(20); fadeAnimator.Play("FadeOut");
        StartCoroutine(QuitAllGame()); Debug.Log("hauste rein"); 
    }
    IEnumerator QuitAllGame() { yield return new WaitForSeconds(2f); Application.Quit(); }
}
