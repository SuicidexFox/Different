using System;
using TMPro;
using UnityEngine;


[Serializable]
public class QuestLine
{
    public TextMeshProUGUI _questText;
}
public class Quest : MonoBehaviour
{
    public QuestLine _questLine;
}