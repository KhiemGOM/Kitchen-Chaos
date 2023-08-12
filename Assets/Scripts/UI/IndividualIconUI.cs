using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualIconUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetIcon(Sprite sprite)
    {
        image.sprite = sprite;
    }
}