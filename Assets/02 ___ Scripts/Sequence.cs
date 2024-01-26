using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sequence : MonoBehaviour
{
    public string scenenmanager;
    public Animator fadeAnimator;
    public Animator rosie;
    public Animator innerChilde;


    private void Start()
    {
        StartCoroutine(StartAnimation());
        GetComponent<Animator>().enabled = true;
    }
    IEnumerator StartAnimation() { yield return new WaitForSeconds(2); fadeAnimator.enabled = false; }

    
    public void Scenenwechsel()
    {
        fadeAnimator.enabled = true;
        fadeAnimator.Play("FadeOut");
        StartCoroutine(CScenenwechsel());
    }
    IEnumerator CScenenwechsel() { yield return new WaitForSeconds(1); SceneManager.LoadScene("SavePlace"); }


    public void Rosie() { rosie.Play("Cry"); }
    public void InnerChilde() {innerChilde.Play("Hallo");}
}
