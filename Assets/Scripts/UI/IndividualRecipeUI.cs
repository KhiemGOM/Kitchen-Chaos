using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndividualRecipeUI : MonoBehaviour
{
    [SerializeField] private GameObject iconTemplate;
    [SerializeField] private TextMeshProUGUI recipeName;

    public void AddIcon (KitchenObjectSO kitchenObjectSO)
    {
        var icon = Instantiate(iconTemplate, transform);
        icon.SetActive(true);
        icon.GetComponent<IndividualIconUI>().SetIcon(kitchenObjectSO.sprite);
    }
    public void SetRecipeName(string newName)
    {
        recipeName.text = newName;
    }
    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
}