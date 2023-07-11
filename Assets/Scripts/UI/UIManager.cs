using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject orderReadyPaper;
    [SerializeField]
    private GameObject orderRequestPaper;

    private Dictionary<int, GameObject> orderRequestDictionary = new Dictionary<int, GameObject>();

    void OnEnable()
    {
        TableBehaviour.OrderTaken += OnOrderTaken;
        TableBehaviour.OrderCancelled += OnOrderCancelled;
        TableBehaviour.OrderDelivered += OnOrderDelivered;
        PlayerBehaviour.OrderRequestedToKitchen += OnOrderRequestedToKitchen;
    }

    void OnDisable()
    {
        TableBehaviour.OrderTaken -= OnOrderTaken;
        TableBehaviour.OrderCancelled -= OnOrderCancelled;
        TableBehaviour.OrderDelivered -= OnOrderDelivered;
        PlayerBehaviour.OrderRequestedToKitchen -= OnOrderRequestedToKitchen;
    }

    private void OnOrderTaken(OrderRequest orderRequest)
    {
        orderRequestDictionary.Add(orderRequest.tableNumber, Instantiate(orderRequestPaper, transform)); 

        var position = transform.position;
        var xOffset = 0;
        foreach (var paper in orderRequestDictionary.Values)
        {
            paper.transform.position = new Vector2 (position.x, position.y);
            xOffset += 10;
        }
    }

    private void OnOrderCancelled(int tableNumber)
    {
        var order = orderRequestDictionary[tableNumber];
        orderRequestDictionary.Remove(tableNumber);
        Destroy(order);
    }

    private void OnOrderDelivered(int tableNumber)
    {
        var order = orderRequestDictionary[tableNumber];
        orderRequestDictionary.Remove(tableNumber);
        Destroy(order);
    }

    private void OnOrderRequestedToKitchen(List<OrderRequest> orderRequests)
    {
        orderRequestDictionary.Clear();
    }
}
