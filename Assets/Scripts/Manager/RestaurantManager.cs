using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static event Action GotFull;
    public static event Action GotFree;

    public List<TableBehaviour> tables = new List<TableBehaviour>();

    void OnEnable()
    {
        SpawnManager.CustomerSpawned += OnCustomerSpawned;
        TableBehaviour.Cleaned += OnTableCleaned;
    }

    void OnDisable()
    {
        SpawnManager.CustomerSpawned -= OnCustomerSpawned;
        TableBehaviour.Cleaned -= OnTableCleaned;
    }

    private void OnCustomerSpawned(CustomerBehaviour customer)
    {

        var freeTables = GetFreeTables();

        if (freeTables.Count == 1) { // Ultima mesa livre
            GotFull?.Invoke();
        }

        customer.SitOnTable(freeTables[UnityEngine.Random.Range(0, freeTables.Count)]);
    }

    private void OnTableCleaned()
    {
        Debug.Log("OnTableCleaned");
        GotFree?.Invoke();
    }

    private List<TableBehaviour> GetFreeTables()
    {
        var list = new List<TableBehaviour>();
        foreach(var table in tables)
        {
            if (table.IsFree())
            {
                list.Add(table);
            }
        }

        return list;
    }
}
