using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private GameObject iconTemplate;
    [SerializeField] private Plate plate;
    private PlateIconUpdater iconUpdater;

    private void Start()
    {
        plate.OnKitchenObjectAdded += OnKitchenObjectAdded;
    }

    private void OnKitchenObjectAdded(object sender, Plate.PlateEventArgs e)
    {
        var icon = Instantiate(iconTemplate, transform);
        icon.SetActive(true);
        icon.GetComponent<PlateIconUpdater>().UpdateIcon(e.ObjectAdded);
    }
}