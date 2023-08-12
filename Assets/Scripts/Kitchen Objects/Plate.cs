using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plate : KitchenObject, IKitchenObjectParent
{
    public class PlateEventArgs : EventArgs
    {
        public KitchenObjectSO ObjectAdded { get; set; }
        public PlateEventArgs(KitchenObjectSO objectAdded)
        {
            ObjectAdded = objectAdded;
        }
    }

    public event EventHandler<PlateEventArgs> OnKitchenObjectAdded;
    public Transform spawnPoint;
    private readonly List<KitchenObjectSO> ingredients = new();
    [SerializeField] private KitchenObjectSO[] validIngredients;

    public override void Update()
    {
        if (Parent is PlateCounter) return;
        transform.position = Parent.GetFollowSpawnPoint().position;
    }
    public List<KitchenObjectSO> GetIngredients()
    {
        return ingredients;
    }
    public bool TryAddIngredient(KitchenObject kitchenObject)
    {
        if (validIngredients.All(n => n != kitchenObject.UnderlyingKitchenObjectSO)) return false;
        if (ingredients.Any(n => n == kitchenObject.UnderlyingKitchenObjectSO)) return false;
        ingredients.Add(kitchenObject.UnderlyingKitchenObjectSO);
        OnKitchenObjectAdded?.Invoke(this, new PlateEventArgs(kitchenObject.UnderlyingKitchenObjectSO));
        kitchenObject.Parent = null;
        kitchenObject.DestroySelf();
        return true;
    }
    public bool CanAddIngredient(KitchenObjectSO kitchenObject)
    {
        return validIngredients.Any(n => n == kitchenObject) && ingredients.All(n => n != kitchenObject);
    }
    public override void DestroySelf()
    {
        Parent.KitchenObject = null;
        Destroy(gameObject);
    }

    public KitchenObject KitchenObject { get; set; }

    public Transform GetFollowSpawnPoint()
    {
        return spawnPoint;
    }
}