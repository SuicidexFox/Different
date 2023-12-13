using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TypwriterEffect : MonoBehaviour
    {
        private readonly TextMeshProUGUI _target;
        private readonly string _text;
        private readonly float _speed;
        private int _currentPosition = -1;
        private bool _hasFinished 
        { get; set; }
        private TypwriterEffect(TextMeshProUGUI Target, string Text, float Speed)
        {
            _target = Target;
            _text = Text;
            _speed = Speed;
        }
        public static TypwriterEffect Start(TextMeshProUGUI Target, string Text, float Speed = 0.03f)
        {
            var effect = new TypwriterEffect(Target, Text, Speed);
            Target.StartCoroutine(effect.Run());
            return effect;
        }
        private IEnumerator Run()
        {
            _target.text = "";
            var textLenght = _text.Length;
            while (!_hasFinished && _currentPosition + 1 < textLenght)
            {
                _target.text += GetNextToken();

                yield return new WaitForSeconds(_speed);
            }
            _hasFinished = true;
        }
        private string GetNextToken()
        {
            _currentPosition++;
            var nextToken = _text[_currentPosition].ToString();
            return nextToken;
        }
    }
}