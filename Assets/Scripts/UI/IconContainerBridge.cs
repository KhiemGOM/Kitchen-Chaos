using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IconContainerBridge : MonoBehaviour
{
    [SerializeField] private IndividualRecipeUI recipe;
    public IndividualRecipeUI Recipe => recipe;
}