using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    public Animator rorschachtest;
    public Animator skillkit;
    public TextMeshProUGUI lettertext;
    
    public RectTransform canvasRectTransform;
    private float movein = 0f;
    private float moveout = 400f;
    private float canvasSpeed = 2f;

    void Start() { canvasRectTransform = GetComponent<RectTransform>(); } 
    
    public void MoveCanvas()
    {
        float targetX = movein;
        float currentX = canvasRectTransform.anchoredPosition.x;
        // Berechne die neue X-Position mit Lerp
        float newX = Mathf.Lerp(currentX, targetX, canvasSpeed * Time.deltaTime);
        // Setze die neue Position zurück auf das RectTransform
        canvasRectTransform.anchoredPosition = new Vector2(newX, canvasRectTransform.anchoredPosition.y);
        // Überprüfe, ob das Ziel erreicht wurde
        if (Mathf.Approximately(newX, targetX))
        {
            GameManager.instance.playerController.questLog = false;
        }
    }
    public void ResetCanvasPosition()
    {
        Vector2 currentPosition = canvasRectTransform.anchoredPosition;
        currentPosition.x = moveout;
        canvasRectTransform.anchoredPosition = currentPosition;
    }

    public void Questende() { StartCoroutine(ActivateBoolForSeconds(2f)); }
    IEnumerator ActivateBoolForSeconds(float seconds)
    { 
        GameManager.instance.playerController.questLog = true;
        yield return new WaitForSeconds(seconds);
        GameManager.instance.playerController.questLog = false;
    }
}



