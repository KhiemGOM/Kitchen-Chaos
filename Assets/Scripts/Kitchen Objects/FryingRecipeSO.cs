using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO from;
    public KitchenObjectSO to;
    public KitchenObjectSO burnt;
    public float fryingTimeGood;
    public float fryingTimeBurnt;
}