using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ContainerCounter : BaseCounter
{
    public event EventHandler PlayerPickUp;
    [SerializeField] private KitchenObjectSO kitchenObjectToSpawnSO;

    public override void Interact(Player player)
    {
        if (player.KitchenObject != null)
        {
            if (player.KitchenObject is Plate playerPlate)
            {
                //Pick object to plate
                if (!playerPlate.CanAddIngredient(kitchenObjectToSpawnSO)) return;
                var temp = KitchenObject.SpawnKitchenObject(kitchenObjectToSpawnSO, spawnPoint.position, playerPlate);
                if (!playerPlate.TryAddIngredient(temp))
                {
                    temp.DestroySelf();
                }
            }
            return;
        }
        //No kitchen object, so spawn one and give to player
        KitchenObject.SpawnKitchenObject(kitchenObjectToSpawnSO, spawnPoint.position, player);
        PlayerPickUp?.Invoke(this, EventArgs.Empty);
    }
    public override string GetInteractionName()
    {
        return Player.Instance.KitchenObject == null ? "Take item" : "None";
    }
}