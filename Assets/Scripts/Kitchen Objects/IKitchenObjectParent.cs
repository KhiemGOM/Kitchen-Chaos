using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public KitchenObject KitchenObject { get; set; }
    public Transform GetFollowSpawnPoint();

    public void ClearKitchenObject()
    {
        if (KitchenObject != null)
            KitchenObject = null;
    }

}