using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderReady
{
    public int tableNumber;
    public int items;
    public OrderReady(int tableNumber, int items)
    {
        this.tableNumber = tableNumber;
        this.items = items;
    }
}
