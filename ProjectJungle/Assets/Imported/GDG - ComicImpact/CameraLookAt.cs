using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteAlways]
public class CameraLookAt : MonoBehaviour
{
    private bool flip;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Application.isPlaying) transform.LookAt(cam.transform.position, Vector3.up);
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
                transform.LookAt(SceneView.GetAllSceneCameras()[0].transform.position, Vector3.up);
#endif
    }
}