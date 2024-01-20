using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TypewriterEffect2 : MonoBehaviour
{
    public float delay = 0.1f;
    public TextMeshProUGUI fullText;
    private string currentText = "";

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.text.Length; i++)
        {
            currentText = fullText.text.Substring(0, i);
            GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}