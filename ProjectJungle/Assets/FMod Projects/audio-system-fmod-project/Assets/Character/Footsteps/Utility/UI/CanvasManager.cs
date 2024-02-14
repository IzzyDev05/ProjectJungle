using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Camera camera;


    private void Awake()
    {
        camera = Camera.main;
        canvas = GetComponent<Canvas>();

        canvas.worldCamera = camera;
    }
}
