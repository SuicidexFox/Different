using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationEventSequence : MonoBehaviour
{
    public Sequence sequence;
    
    public void Rosie()
    {
        sequence.Rosie();
    }
    
    public void InnerChilde()
    {
        sequence.InnerChilde();
    }
}
