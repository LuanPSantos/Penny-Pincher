using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    private List<OrderRequest> orderQueue = new List<OrderRequest>();
    private List<OrderReady> ordersReady = new List<OrderReady>();

    private float timer = 0;
    private int waitingTime = 5;
    [SerializeField]
    private TMPro.TextMeshPro orderQueueText;
    [SerializeField]
    private TMPro.TextMeshPro orderReadyText;

    void OnEnable()
    {
        TableBehaviour.OrderCancelled += OnOrderCancelled;
    }

    void OnDisabled()
    {
        TableBehaviour.OrderCancelled -= OnOrderCancelled;
    }


    void Update()
    {
        orderQueueText.text = "Preparing " + orderQueue.Count.ToString();
        orderReadyText.text = "Ready " + ordersReady.Count.ToString();

        if (orderQueue.Count > 0)
        {
            timer += Time.deltaTime;

            if (timer > waitingTime)
            {
                Debug.Log("orderQueue.Count " + orderQueue.Count);
                timer = 0;
                var orderRequest = orderQueue[orderQueue.Count - 1];

                orderQueue.Remove(orderRequest);

                waitingTime = orderRequest.items;

                Debug.Log("Order Processing " + orderRequest.items);

                ordersReady.Add(new OrderReady(orderRequest.tableNumber, orderRequest.items));

                Debug.Log("Order Ready");
            }

            orderQueueText.text = "Preparing " + orderQueue.Count.ToString();
            orderReadyText.text = "Ready " + ordersReady.Count.ToString();
        }
    }

    public void PrepareOrders(List<OrderRequest> orderRequests)
    {
        orderQueue.AddRange(orderRequests);
    }


    public List<OrderReady> GetOrdersReady()
    {
        var orders = ordersReady;
        ordersReady = new List<OrderReady>();

        return orders;
    }

    private void OnOrderCancelled(int tableNumber)
    {
        var orderReadyToRemove = ordersReady.Find((order) => order.tableNumber == tableNumber);
        ordersReady.Remove(orderReadyToRemove);

        var orderRequestToRemove = orderQueue.Find((order) => order.tableNumber == tableNumber);
        orderQueue.Remove(orderRequestToRemove);
    }
}
