using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public class OrderChangeEventArgs : System.EventArgs
    {
        public readonly bool isAdded; // true if added, false if removed
        public readonly int indexChange;
        public readonly RecipeSO recipeIfAdd;

        public OrderChangeEventArgs(bool isAdded, int indexChange, RecipeSO recipeIfAdd)
        {
            this.isAdded = isAdded;
            this.indexChange = indexChange;
            this.recipeIfAdd = recipeIfAdd;
        }
    }

    public event EventHandler<OrderChangeEventArgs> OnOrderChange;
    [SerializeField] private List<RecipeSO> validOrders;
    [SerializeField] private float orderTimeMax = 8f;
    [SerializeField] private float orderTimeMin = 2f;
    [SerializeField] private int maxOrder = 4;
    private List<RecipeSO> orders;
    private float orderTimer;
    private bool firstOrder = true;

    public static DeliveryManager Instance { get; private set; }

    private void ResetOderTime ()
    {
        orderTimer = Random.Range(orderTimeMin, orderTimeMax);
    }
    private void Awake()
    {
        Instance = this;
    }

    public static T DebugLog<T>(T obj, string prefix = "")
    {
        Debug.Log(prefix + obj);
        return obj;
    }

    private void Start()
    {
        orders = new List<RecipeSO>();
        ResetOderTime();
    }

    private void Update()
    {
        if (KitchenGameManager.Instance.State != KitchenGameManager.GameState.Playing) return;
        if (orders.Count >= maxOrder) return;
        orderTimer -= Time.deltaTime;
        if (orderTimer > 0 && !firstOrder) return;
        if (firstOrder) firstOrder = false;
        ResetOderTime();
        orders.Add(validOrders[Random.Range(0, validOrders.Count)]);
        SFXManager.Instance.PlaySound(SFXManager.SFXType.DeliveryRing, Vector3.zero, 0.3f);
        OnOrderChange?.Invoke(this, new OrderChangeEventArgs(true, orders.Count - 1, orders.Last()));
    }

    private static bool SameList<T>(IReadOnlyCollection<T> list1, IReadOnlyCollection<T> list2)
    {
        return list1.Count == list2.Count && list1.All(item => list2.Any(r => r.Equals(item)));
    }

    public bool DeliverOrder(Plate plate)
    {
        var serveCounter = ServeCounter.Instance;
        if (orders.All(recipe => !SameList(recipe.ingredients, plate.GetIngredients())))
        {
            SFXManager.Instance.PlaySound(SFXManager.SFXType.DeliveryFail, serveCounter.transform.position);
            KitchenGameManager.Instance.Score--;
            serveCounter.ShowDeliveryFailUI();
            return false;
        }

        var correctOrder = orders.First(r => SameList(r.ingredients, plate.GetIngredients()));
        OnOrderChange?.Invoke(this, new OrderChangeEventArgs(false, orders.IndexOf(correctOrder), null));
        orders.Remove(correctOrder);
        SFXManager.Instance.PlaySound(SFXManager.SFXType.DeliverySuccess, serveCounter.transform.position);
        serveCounter.ShowDeliverySuccessUI();
        //Add score = to the amount of ingredient
        KitchenGameManager.Instance.Score+= correctOrder.ingredients.Count;
        return true;
    }
}