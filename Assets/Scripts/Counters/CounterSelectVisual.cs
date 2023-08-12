using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSelectVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private Transform[] selectComponents;

    private void Start()
    {
        Player.Instance.OnSelect += OnSelect;
    }

    private void OnSelect(object sender, Player.SelectEventArgs e)
    {
        SetActiveSelectComponent(e.SelectedCounter == counter);
    }

    private void SetActiveSelectComponent(bool val)
    {
        foreach (var instance in selectComponents)
        {
            instance.gameObject.SetActive(val);
        }
    }
}