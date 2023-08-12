using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterWarningUI : MonoBehaviour
{
    [SerializeField] private GameObject warningUI;

    public bool IsShow
    {
        get => warningUI.activeSelf;
        set => warningUI.SetActive(value);
    }
}