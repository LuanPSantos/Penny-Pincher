using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TableBehaviour : MonoBehaviour
{
    public static event Action<OrderRequest> OrderTaken;
    public static event Action<int> MoneyTaken;
    public static event Action<int> Closed;
    public static event Action Cleaned;
    public static event Action<int> OrderCancelled;
    public static event Action<int> OrderDelivered;

    [SerializeField]
    public int tableNumber;
    [SerializeField]
    private int minPatiace;
    [SerializeField]
    private int maxPatiace;
    [SerializeField]
    private TMPro.TextMeshPro statusText;

    private OrderRequest orderRequest = null;
    private OrderReady orderReady = null;
    private State state = State.FREE;
    private int money = 0;

    private bool hasInteracted = false;
    private int waitingTime;
    private float timer;

    void Start()
    {
        state = State.FREE;
    }
    
    void Update()
    {
        statusText.text = state.ToString();
        switch (state)
        {
            case State.FREE:
                OnFree();
                break;
            case State.IN_USE:
                OnInUse();
                break;
            case State.CHOOSING:
                OnChoosing();
                break;
            case State.WAITING_WAITER:
                OnWaintingWaiter();
                break;
            case State.WAITING_ORDER:
                OnWaitOrderToBeDelivered();
                break;
            case State.CONSUMING_ORDER:
                OnConsumingOrder();
                break;
            case State.DIRTY:
                OnDurty();
                break;
            default: break;
        }
    }

    public void Interact(List<OrderReady> readyOrders)
    {
        hasInteracted = true;

        foreach (OrderReady orderReady in readyOrders)
        {
            if(orderReady.tableNumber == tableNumber)
            {
                this.orderReady = orderReady;
                OrderDelivered?.Invoke(tableNumber);
            }
        }
    }

    public void SinalyzeOrderWaiting(OrderRequest order)
    {
        orderRequest = order;
    }

    public bool IsFree()
    {
        return state == State.FREE; 
    }

    public void Reserve()
    {
        state = State.IN_USE;
    }

    public void Sit()
    {
        StartChoosing();
    }

    public void Leave()
    {
        state = State.DIRTY;

    }

    public void Pay()
    {
        money = 10;
    }

    public void Clean()
    {
        state = State.FREE;
        Cleaned?.Invoke();
        Debug.Log(state);
    }

    private void OnFree()
    {
        if(hasInteracted)
        {
            hasInteracted = false;
        }
    }

    private void OnInUse()
    {
        if (hasInteracted)
        {
            hasInteracted = false;
        }
    }

    private void OnWaintingWaiter()
    {
        if (hasInteracted)
        {
            hasInteracted = false;

            ReadOrder();
            WaitOrderToBeDelivered();
        }

        timer += Time.deltaTime;

        if (timer > waitingTime) // Se esperou demais
        {
            timer = 0;
            LeaveWithoutPaying();
        }
    }

    private void OnDurty()
    {
        if (hasInteracted)
        {
            hasInteracted = false;

            TakeMoney();
            Clean();
        }
    }

    private void OnChoosing()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime) // Se Deu tempo para escolher
        {
            timer = 0;
            Choose();
        }
    }

    private void OnWaitOrderToBeDelivered()
    {
        if(orderReady != null && hasInteracted)
        {
            hasInteracted = false;
            Consume(orderReady);
        }

        timer += Time.deltaTime;

        if (timer > waitingTime) // Se esperou demais
        {
            timer = 0;
            LeaveWithoutPaying();
        }
    }

    private void OnConsumingOrder()
    {
        timer += Time.deltaTime;

        if (timer > waitingTime) // Ja comeu
        {
            timer = 0;
            PayAndLeave();
        }
    }

    private void ReadOrder()
    {
        OrderTaken?.Invoke(orderRequest);
        orderRequest = null;
    }

    private void TakeMoney()
    {
        MoneyTaken?.Invoke(money);
    }

    private void WaitForTheWaiter()
    {
        state = State.WAITING_WAITER;
        timer = 0;
        waitingTime = UnityEngine.Random.Range(minPatiace, maxPatiace);
    }

    private void WaitOrderToBeDelivered()
    {
        state = State.WAITING_ORDER;
        timer = 0;
        waitingTime = UnityEngine.Random.Range(minPatiace, maxPatiace);
    }

    private void StartChoosing()
    {
        state = State.CHOOSING;
        timer = 0;
        waitingTime = UnityEngine.Random.Range(minPatiace, maxPatiace);
    }

    private void Choose()
    {
        var order = new OrderRequest(tableNumber, UnityEngine.Random.Range(1, 10));

        SinalyzeOrderWaiting(order);
        WaitForTheWaiter();
    }    

    private void Consume(OrderReady order) // TODO consumo dinamico
    {
        state = State.CONSUMING_ORDER;
        timer = 0;
        waitingTime = UnityEngine.Random.Range(minPatiace, maxPatiace);
    }

    private void LeaveWithoutPaying()
    {
        Leave();
        Closed?.Invoke(tableNumber);
        OrderCancelled?.Invoke(tableNumber);
    }

    private void PayAndLeave()
    {
        Pay();
        Leave();
        Closed?.Invoke(tableNumber);
    }

    enum State
    {
        FREE, IN_USE, CHOOSING, WAITING_WAITER, WAITING_ORDER, CONSUMING_ORDER, DIRTY
    }
}
