using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter
{
    public event EventHandler PlayerCut;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;
    [SerializeField] private CounterProgressBar progressBar;
    private int cuttingCount;

    public override void Interact(Player player)
    {
        if (KitchenObject == null)
        {
            if (player.KitchenObject == null) return;
            if (cuttingRecipes.All(r => r.from != player.KitchenObject.UnderlyingKitchenObjectSO)) return;
            player.KitchenObject.Parent = this;
            cuttingCount = 0;
            progressBar.SetProgress(0);
        }
        else
        {
            if (player.KitchenObject != null)
            {
                if (player.KitchenObject is Plate playerPlate)
                {
                    //Pick object to plate
                    playerPlate.TryAddIngredient(KitchenObject);
                }

                cuttingCount = 0;
                progressBar.SetProgress(0);
                return;
            }

            KitchenObject.Parent = player;
            cuttingCount = 0;
            progressBar.SetProgress(0);
        }
    }

    public override void InteractAlt(Player player)
    {
        if (KitchenObject == null) return;
        //Chopping
        SFXManager.Instance.PlaySound(SFXManager.SFXType.Chop, transform.position);
        foreach (var recipe in cuttingRecipes)
        {
            if (recipe.from != KitchenObject.UnderlyingKitchenObjectSO) continue;
            cuttingCount++;
            progressBar.SetProgress((float)cuttingCount / recipe.cuttingRequired);
            PlayerCut?.Invoke(this, EventArgs.Empty);
            if (cuttingCount >= recipe.cuttingRequired)
            {
                //Done chopping
                KitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(recipe.to, spawnPoint.position, this);
                break;
            }
        }
    }

    public override string GetInteractionName()
    {
        if (KitchenObject == null && Player.Instance.KitchenObject != null &&
            cuttingRecipes.Any(r => r.from == Player.Instance.KitchenObject.UnderlyingKitchenObjectSO))
        {
            return "Put down";
        }

        if (KitchenObject != null && Player.Instance.KitchenObject == null)
        {
            return "Pick up";
        }

        return "None";
    }
}