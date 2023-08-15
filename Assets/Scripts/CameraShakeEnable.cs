using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraShakeEnable : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private const string GAME_CAMERA_SHAKE = "GameCameraShake";

    public bool IsCameraShakeEnable
    {
        get => cinemachineBasicMultiChannelPerlin.enabled;
        private set
        {
            cinemachineBasicMultiChannelPerlin.enabled = value;
            PlayerPrefs.SetInt(GAME_CAMERA_SHAKE, value ? 1 : 0);
        }
    }

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        IsCameraShakeEnable = PlayerPrefs.GetInt(GAME_CAMERA_SHAKE, 1) == 1;
        if (toggle == null) return;
        toggle.isOn = PlayerPrefs.GetInt(GAME_CAMERA_SHAKE, 1) == 1;
        toggle.onValueChanged.AddListener(isCameraShakeEnable =>
        {
            if (cinemachineBasicMultiChannelPerlin != null)
                IsCameraShakeEnable = isCameraShakeEnable;
        });
    }
}