using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.KitchenObject == null) return;
        SFXManager.Instance.PlaySound(SFXManager.SFXType.Trash, transform.position);
        player.KitchenObject.DestroySelf();
    }
    public override string GetInteractionName()
    {
        if (KitchenObject == null && Player.Instance.KitchenObject != null)
        {
            return "Throw away";
        }
        return "None";
    }
}