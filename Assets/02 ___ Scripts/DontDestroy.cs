using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy keeper;
    private void Awake()
    {
        if (keeper != null) { GameObject.Destroy(this.gameObject); return; }
        else { keeper = this; }
        DontDestroyOnLoad(this);
        RuntimeManager.GetBus("bus:/Master").setVolume(0.5f);
        RuntimeManager.GetBus("bus:/Master/Music").setVolume(0.5f);
        RuntimeManager.GetBus("bus:/Master/SFX").setVolume(0.5f);
    }
}
