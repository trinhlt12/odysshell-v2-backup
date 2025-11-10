using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirtualCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
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
            this._virtualCamera.GetCinemachineComponent<CinemachinePOV>().enabled = false;
        }
        else
        {
            this._virtualCamera.GetCinemachineComponent<CinemachinePOV>().enabled = true;
        }
    }
}