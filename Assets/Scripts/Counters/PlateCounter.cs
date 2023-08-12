using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    private readonly List<KitchenObject> plates = new();
    [SerializeField] private KitchenObjectSO plate;
    [SerializeField] private float spawnTime = 4f;
    [SerializeField] private int maxPlate = 3;
    private float spawnTimer;
    private const float SPAWN_OFFSET = 0.1f;

    private void Update()
    {
        if (KitchenGameManager.Instance.State != KitchenGameManager.GameState.Playing) return;
        if (plates.Count >= maxPlate) return;
        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnTime) return;
        spawnTimer = 0f;
        KitchenObject = null;
        plates.Insert(plates.Count,
            KitchenObject.SpawnKitchenObject(plate, spawnPoint.position + plates.Count * SPAWN_OFFSET * Vector3.up,
                this));
    }

    public override void Interact(Player player)
    {
        if (KitchenObject == null) return;
        if (player.KitchenObject != null)
        {
            //Pick object to plate and give plate to player
            var pickUpPlate = plates[^1] as Plate;
            if (pickUpPlate == null)
            {
                return;
            }

            if (pickUpPlate.TryAddIngredient(player.KitchenObject))
            {
                //Put object to plate
                pickUpPlate.Parent = player;
                plates.RemoveAt(plates.Count - 1);
                if (plates.Count > 0)
                {
                    KitchenObject = plates[^1];
                }
            }

            return;
        }

        KitchenObject.Parent = player;
        plates.RemoveAt(plates.Count - 1);
        if (plates.Count > 0)
        {
            KitchenObject = plates[^1];
        }
    }
    public override string GetInteractionName()
    {
        if (KitchenObject != null && Player.Instance.KitchenObject == null)
        {
            return "Take plate";
        }
        if (KitchenObject != null && Player.Instance.KitchenObject != null)
        {
            return "Put to plate";
        }
        return "None";
    }
}