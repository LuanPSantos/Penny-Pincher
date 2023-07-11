using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static event Action<List<OrderRequest>> OrderRequestedToKitchen;
    public static event Action<OrderReady> GotOrderReady;

    [SerializeField]
    private KitchenManager kitchenManager;
    [SerializeField]
    private TMPro.TextMeshPro ordersText;

    private TableBehaviour tableNear = null;

    private List<OrderRequest> ordersToKitchen = new List<OrderRequest>();
    private List<OrderReady> ordersReady = new List<OrderReady>();

    private bool onOrderReceiver = false;
    private bool onOrderDispatcher = false;
    


    void OnEnable()
    {
        TableBehaviour.OrderTaken += OnOrderTaken;
        TableBehaviour.OrderCancelled += OnOrderCancelled;
        TableBehaviour.OrderDelivered += OnOrderDelivered;
        GameManager.FistPeriodStarted += OnFistPeriodStarted;
    }

    void OnDisable()
    {
        TableBehaviour.OrderTaken -= OnOrderTaken;
        TableBehaviour.OrderCancelled -= OnOrderCancelled;
        TableBehaviour.OrderDelivered -= OnOrderDelivered;
        GameManager.FistPeriodStarted -= OnFistPeriodStarted;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        ordersText.text = "R: " + ordersToKitchen.Count.ToString() + " | D: " + ordersReady.Count.ToString();

        if (Input.GetKeyDown(KeyCode.W))
        {
            tableNear?.Interact(ordersReady);
        }

        if (Input.GetKeyDown(KeyCode.W) && onOrderReceiver)
        {
            var copy = ordersToKitchen;
            ordersToKitchen = new List<OrderRequest>();
            OrderRequestedToKitchen?.Invoke(copy);
            kitchenManager.PrepareOrders(copy);
        }

        if (Input.GetKeyDown(KeyCode.W) && onOrderDispatcher)
        {
            ordersReady.AddRange(kitchenManager.GetOrdersReady());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Table"))
        {
            tableNear = collision.gameObject.GetComponent<TableBehaviour>();
        }

        if (collision.CompareTag("OrderReceiver"))
        {
            onOrderReceiver = true;
        }

        if (collision.CompareTag("OrderDispatcher"))
        {
            onOrderDispatcher = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Table"))
        {
            tableNear = null;
        }
        if (collision.CompareTag("OrderReceiver"))
        {
            onOrderReceiver = false;
        }

        if (collision.CompareTag("OrderDispatcher"))
        {
            onOrderDispatcher = false;
        }
    }

    private void OnOrderTaken(OrderRequest orderRequest)
    {
        ordersToKitchen.Add(orderRequest);
    }

    private void OnOrderCancelled(int tableNumber)
    {
        RemoveOrderByTableNumber(tableNumber);
    }

    private void OnOrderDelivered(int tableNumber)
    {
        RemoveOrderByTableNumber(tableNumber);
    }

    private void RemoveOrderByTableNumber(int tableNumber)
    {
        var orderReadyToRemove = ordersReady.Find((order) => order.tableNumber == tableNumber);
        ordersReady.Remove(orderReadyToRemove);

        var orderRequestToRemove = ordersToKitchen.Find((order) => order.tableNumber == tableNumber);
        ordersToKitchen.Remove(orderRequestToRemove);

    }

    private void OnFistPeriodStarted()
    {
        GetComponent<PlayerMovementBehaviour>().enabled = true;
    }
}
