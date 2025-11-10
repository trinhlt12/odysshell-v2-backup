using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreelookCameraController : MonoBehaviour
{
    private CinemachineFreeLook _freelookCamera;

    private void Awake()
    {
        this._freelookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var currentSsene = scene.name;
        if (currentSsene == "FirstScene")
        {
            this._freelookCamera.enabled = false;
        }
        else
        {
            this._freelookCamera.enabled = true;
        }
    }
}