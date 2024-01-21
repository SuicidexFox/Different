using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CurrentButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonTextUI;
    public string text;
    public UnityEvent buttonEvent;
    
    public void Setup(string text, UnityEvent buttonEvent)
    {
        this.text = text;
        this.buttonEvent = buttonEvent;
        _buttonTextUI.SetText(text);
    }
    public void OnClick()
    {
        if (buttonEvent.GetPersistentEventCount() == 0)
        {
            GameManager.instance.NextDialogLine();
            return;
        }
        buttonEvent.Invoke();
    }
}
