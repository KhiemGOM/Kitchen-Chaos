using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderCallback : MonoBehaviour
{
    private bool isFirstFrame = true;

    private void Update()
    {
        if (!isFirstFrame) return;
        SceneLoader.SceneLoaderCallback();
        isFirstFrame = false;
    }
}