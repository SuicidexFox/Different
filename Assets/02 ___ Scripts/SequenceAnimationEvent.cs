using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimationEventSequence : MonoBehaviour
{
    public Sequence sequence;
    
    public void RosieSequence() { sequence.rosie.Play("Sequence"); }
    public void RosieHallo() { sequence.rosie.Play("winken");}
    public void ChildHallo() { sequence.innerChilde.Play("Hallo"); }
    public void ChildKlatschen() { sequence.innerChilde.Play("Klatschen"); }
    
    public void FadeOut() { sequence.Scenenwechsel(); }
}
