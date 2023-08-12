using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitching : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineVirtualCamera vCam;
    void Start()
    {
        vCam = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            vCam.enabled = !vCam.enabled;
        }
    }
}