using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ServeCounter : BaseCounter
{
    private DeliveryManager deliveryManager;
    public static ServeCounter Instance { get; private set; }
    [SerializeField] private GameObject deliverySuccessUI;
    [SerializeField] private GameObject deliveryFailUI;
    private Animator deliverySuccessUIAnimator;
    private Animator deliveryFailUIAnimator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        deliverySuccessUIAnimator = deliverySuccessUI.GetComponent<Animator>();
        deliveryFailUIAnimator = deliveryFailUI.GetComponent<Animator>();
        deliveryManager = DeliveryManager.Instance;
    }

    private void Update()
    {
        if (deliverySuccessUI.activeSelf &&
            deliverySuccessUIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            deliverySuccessUI.SetActive(false);
        }

        if (deliveryFailUI.activeSelf && deliveryFailUIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            deliveryFailUI.SetActive(false);
        }
    }

    public void ShowDeliverySuccessUI()
    {
        deliverySuccessUI.SetActive(true);
    }

    public void ShowDeliveryFailUI()
    {
        deliveryFailUI.SetActive(true);
    }

    public override void Interact(Player player)
    {
        if (player.KitchenObject == null || player.KitchenObject is not Plate plate) return;
        //Review food to give score
        deliveryManager.DeliverOrder(plate);
        plate.DestroySelf();
    }

    public override string GetInteractionName()
    {
        if (Player.Instance.KitchenObject == null) return "None";
        return Player.Instance.KitchenObject is Plate ? "Serve" : "None";
    }
}