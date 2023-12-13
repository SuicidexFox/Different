using System.Collections.Generic;
using System.Collections;
using UnityEngine;



[CreateAssetMenu(menuName = "Data/NewImage")]
    public class ScriptableImage : ScriptableObject
    {
        public Sprite Image;
        public int ID;
    }