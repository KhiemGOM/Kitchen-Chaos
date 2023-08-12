using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private enum FaceCameraMode
    {
        LookAt,
        LookAway
    }

    [SerializeField] private FaceCameraMode faceCameraMode = FaceCameraMode.LookAt;
    private Vector3 mainCamPosition;

    private void Start()
    {
        if (Camera.main != null) mainCamPosition = Camera.main.transform.position;
    }

    private void LateUpdate()
    {
        if (Camera.main == null) return;
        switch (faceCameraMode)
        {
            case FaceCameraMode.LookAt:
                transform.LookAt(mainCamPosition, Vector3.up);
                break;
            case FaceCameraMode.LookAway:
                transform.LookAt(transform.position * 2 - mainCamPosition, Vector3.up);
                break;
        }
    }
}