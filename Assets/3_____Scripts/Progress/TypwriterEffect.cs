using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/*
public class TypewriterEffect : MonoBehaviour

{
    private float _speed = 0.1f;
    private int _currentPosition = -1;
    private bool _hasFinished { get; set; }
    private void Start()
    { StartCoroutine(Run(_text:)); }
    private IEnumerator Run(DialoguesLines _text)
    {
        var textLenght = _text._text.Length;
        while (!_hasFinished && _currentPosition + 1 < textLenght)
        {
            _text._text += GetNextToken(_text);
            yield return new WaitForSeconds(_speed);
        }
        _hasFinished = true;
    }
    private string GetNextToken(DialoguesLines dialoguesLines)
    {
        _currentPosition++;
        var nextToken = dialoguesLines._text[_currentPosition].ToString();
        return nextToken;
    } 
}




/*IEnumerator TypewriterEffect(DialoguesLines dialoguesLines)
{
    textDialog.text = "";
    var textLenght = _string.Length;
    while (!_finish && _textPosition + -1 < textLenght)
    {
        _string += GetNextToken();
        yield return new WaitForSeconds(_textSpeed);
    }
    _finish = true;
}
private string GetNextToken()
{
    _textPosition++;
    var nextToken = _string[_textPosition].ToString();
    return nextToken;
}*/