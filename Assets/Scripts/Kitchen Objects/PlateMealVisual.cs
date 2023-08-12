using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateMealVisual : MonoBehaviour
{
    [Serializable]
    public struct MealVisual
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject visual;
    }

    [SerializeField] private MealVisual[] typeToVisual;
    [SerializeField] private Plate plate;

    private void Start()
    {
        plate.OnKitchenObjectAdded += OnKitchenObjectAdded;
    }

    private void OnKitchenObjectAdded(object sender, Plate.PlateEventArgs e)
    {
        foreach (var mealVisual in typeToVisual)
        {
            if (mealVisual.kitchenObjectSO == e.ObjectAdded)
            {
                mealVisual.visual.SetActive(true);
            }
        }
    }
}