using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] List<CinemachineVirtualCameraBase> virtualCameras;

    Camera activeCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (virtualCameras == null || virtualCameras.Count == 0)
        {
            Debug.LogError("No virtual cameras assigned in CameraManager.");
            return;
        }

        SetActiveCamera(0);
    }

    public Camera GetActiveCamera()
    {
        return activeCamera;
    }

    public void SwitchCamera(int index)
    {
        if (index < 0 || index >= virtualCameras.Count)
        {
            Debug.LogError($"Invalid camera index: {index}. Must be between 0 and {virtualCameras.Count - 1}.");
            return;
        }

        SetActiveCamera(index);
    }

    private void SetActiveCamera(int index)
    { 
        for (int i = 0; i < virtualCameras.Count; i++)
        {
            if (virtualCameras[i] != null)
            {
                virtualCameras[i].gameObject.SetActive(i == index);
                activeCamera = virtualCameras[i].GetComponent<Camera>();
            }
        }
    }
}
