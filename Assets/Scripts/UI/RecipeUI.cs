using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private GameObject recipeTemplate;
    private DeliveryManager deliveryManager;
    private List<Transform> Recipes { get; set; }

    private void Start()
    {
        Recipes = new List<Transform>();
        deliveryManager = DeliveryManager.Instance;
        deliveryManager.OnOrderChange += OnOrderChange;
    }

    private void OnOrderChange(object sender, DeliveryManager.OrderChangeEventArgs e)
    {
        if (e.isAdded)
        {
            var recipe = Instantiate(recipeTemplate, transform);
            recipe.gameObject.SetActive(true);
            Recipes.Add(recipe.transform);
            var recipeUI = recipe.GetComponent<IconContainerBridge>().Recipe;
            recipeUI.SetRecipeName(e.recipeIfAdd.recipeName);
            foreach (var ingredient in e.recipeIfAdd.ingredients)
            {
                recipeUI.AddIcon(ingredient);
            }
        }
        else
        {
            Recipes[e.indexChange].GetComponent<IconContainerBridge>().Recipe.DestroySelf();
            Recipes.RemoveAt(e.indexChange);
        }
    }
}