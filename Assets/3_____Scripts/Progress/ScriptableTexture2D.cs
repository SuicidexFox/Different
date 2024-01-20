using System.Collections.Generic;
using System.Collections;
using FMODUnity;
using UnityEngine;



[CreateAssetMenu(menuName = "Data/NewImage")]
public class ScriptableImage : ScriptableObject
{
    public Texture2D Image;
    public string ID;
}
    
    
[CreateAssetMenu(menuName = "Data/Music")]
public class ScriptableMusicState : ScriptableObject
{
    public EventReference Music;
    public int ID;
}