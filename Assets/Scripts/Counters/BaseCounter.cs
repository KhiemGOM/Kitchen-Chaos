using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public abstract void Interact(Player player);

    public virtual void InteractAlt(Player player) {}
    [SerializeField] protected Transform spawnPoint;
    public Transform GetFollowSpawnPoint()
    {
        return spawnPoint;
    }

    public abstract string GetInteractionName();
    public KitchenObject KitchenObject { get; set; }

}