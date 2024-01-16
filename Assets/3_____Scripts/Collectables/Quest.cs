using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;


[Serializable]
public class QuestLine
{
    public TextMeshProUGUI _questText;
}


public class Quest : MonoBehaviour

{
    public QuestLine _questLine;
}