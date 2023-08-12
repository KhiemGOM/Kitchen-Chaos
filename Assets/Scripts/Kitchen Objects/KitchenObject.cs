using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO underlyingKitchenObjectSO;
    private IKitchenObjectParent parent;

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO objectToSpawn, Vector3 position,
        IKitchenObjectParent parent)
    {
        var kitchenObjectTransform =
            Instantiate(objectToSpawn.prefab, position, Quaternion.identity);
        var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.Parent = parent;
        return kitchenObject;
    }

    public IKitchenObjectParent Parent
    {
        get => parent;
        //Will also set the parent's kitchen object to this
        set
        {
            //Play sound
            if (value is Player player)
            {
                SFXManager.Instance.PlaySound(SFXManager.SFXType.ObjectPickUp, player.transform.position);
            }
            else
            {
                SFXManager.Instance.PlaySound(SFXManager.SFXType.ObjectDrop, transform.position);
            }

            //Old parent clear
            parent?.ClearKitchenObject();
            if (value?.KitchenObject != null)
            {
                Debug.LogError("Cannot set parent to an parent-able object that already has a kitchen object!");
                return;
            }

            //New parent = set value
            parent = value;
            if (parent != null) parent.KitchenObject = this;
        }
    }

    public KitchenObjectSO UnderlyingKitchenObjectSO => underlyingKitchenObjectSO;

    public virtual void DestroySelf()
    {
        if (Parent != null)
            Parent.KitchenObject = null;
        Destroy(gameObject);
    }

    public virtual void Update()
    {
        if (Parent == null) Destroy(gameObject);
        transform.position = Parent.GetFollowSpawnPoint().position;
    }
}