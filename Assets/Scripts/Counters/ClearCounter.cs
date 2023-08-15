using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (KitchenObject == null)
        {
            //Nothing on counter
            if (player.KitchenObject != null)
            {
                //Put object down
                player.KitchenObject.Parent = this;
            }
        }
        else
        {
            //Something on counter
            if (player.KitchenObject == null)
            {
                //Pick object up
                KitchenObject.Parent = player;
            }
            else
            {
                if (KitchenObject is Plate counterPlate)
                {
                    //Put object down to plate
                    counterPlate.TryAddIngredient(player.KitchenObject);
                }

                if (player.KitchenObject is Plate playerPlate)
                {
                    //Pick object to plate
                    playerPlate.TryAddIngredient(KitchenObject);
                }
            }
        }
    }

    public override string GetInteractionName()
    {
        if (KitchenObject == null && Player.Instance.KitchenObject != null)
        {  
            return "Put down";
        }
        if (KitchenObject != null && Player.Instance.KitchenObject != null)
        {
            if (KitchenObject is Plate plate && plate.CanAddIngredient(Player.Instance.KitchenObject.UnderlyingKitchenObjectSO))
            {
                return "Put down";
            }

            if (Player.Instance.KitchenObject is Plate plate2 && plate2.CanAddIngredient(KitchenObject.UnderlyingKitchenObjectSO))
            {
                return "Pick up";
            }
        }
        if (KitchenObject != null && Player.Instance.KitchenObject == null)
        {
            return "Pick up";
        }
        return "None";
    }
}