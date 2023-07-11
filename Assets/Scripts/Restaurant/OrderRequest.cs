using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderRequest
{
    public int tableNumber;
    public int items;
    State state;

    public OrderRequest(
        int tableFrom,
        int items)
    {
        this.tableNumber = tableFrom; 
        this.items = items;
        state = State.WAITING_KITCHEN;
    }

    public bool IsReady()
    {
        return state == State.READY;
    }

    public void Prepare()
    {
        state = State.BEEN_PARARED;
    }

    public void PutToBeDelivered()
    {
        state = State.READY;
    }

    enum State
    {
        WAITING_KITCHEN, BEEN_PARARED, READY
    }
}
