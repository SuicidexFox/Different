using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class BackgroundRain : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private MeshRenderer meshRenderer;
    private Vector2 _savedOffset;


    private void Start()
    {
        _savedOffset = meshRenderer.sharedMaterial.mainTextureOffset;
    }

    private void Update()
    {
        var y = Mathf.Repeat(Time.time * speed, 1);
        var offset = new Vector2(_savedOffset.x, y);
        meshRenderer.sharedMaterial.mainTextureOffset = offset;
    }
    private void OnDisable()
    {
        meshRenderer.sharedMaterial.mainTextureOffset = _savedOffset;
    }
}
