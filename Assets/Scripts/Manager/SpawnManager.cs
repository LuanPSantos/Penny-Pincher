using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static event Action<CustomerBehaviour> CustomerSpawned;
    public static event Action AllCustomerSpawned;

    [SerializeField]
    private GameObject customerPrefab;
    [SerializeField]
    private Transform spawner;

    private float ratioOfPawn = 1;

    private float currentTTL = 0;
    private bool isSpawning = false;
    private bool enabledToSpawn = true;
    private int customersToSpawn = 0;
    private int customersSpawned = 0;

    void OnEnable()
    {
        GameManager.FistPeriodStarted += StartFirstPawningCycle;
        GameManager.FistPeriodEnded += StopSpawning;
        GameManager.SecondPeriodStarted += StartSecondPawningCycle;
        GameManager.SecondPeriodEnded += StopSpawning;
        GameManager.ThirdPeriodStarted += StartThidPawningCycle;
        GameManager.ThirdPeriodEnded += StopSpawning;
        RestaurantManager.GotFull += OnRestaurantGotFull;
        RestaurantManager.GotFree += OnRestaurantGotFree;
        CustomerBehaviour.Left += OnCustomerLeft;
    }


    void OnDisable()
    {
        GameManager.FistPeriodStarted -= StartFirstPawningCycle;
        GameManager.FistPeriodEnded -= StopSpawning;
        GameManager.SecondPeriodStarted -= StartSecondPawningCycle;
        GameManager.SecondPeriodEnded -= StopSpawning;
        GameManager.ThirdPeriodStarted -= StartThidPawningCycle;
        GameManager.ThirdPeriodEnded -= StopSpawning;
        RestaurantManager.GotFull -= OnRestaurantGotFull;
        RestaurantManager.GotFree -= OnRestaurantGotFree;
        CustomerBehaviour.Left -= OnCustomerLeft;
    }

    void Update()
    {
        currentTTL += Time.deltaTime;

        if(enabledToSpawn && isSpawning && currentTTL > ratioOfPawn && customersSpawned < customersToSpawn)
        {
            var objectSpawned = Instantiate(customerPrefab, spawner.position, Quaternion.identity);
            var customer = objectSpawned.GetComponent<CustomerBehaviour>();
            customer.SetExitDoor(spawner);

            CustomerSpawned?.Invoke(customer);
            currentTTL = 0;
            customersSpawned++;
        }
    }

    public void StartFirstPawningCycle()
    {
        isSpawning = true;
        Debug.Log("StartFirstPawningCycle");
        ratioOfPawn = 4;
        customersToSpawn = 4;
        customersSpawned = 0;
    }

    public void StartSecondPawningCycle()
    {
        isSpawning = true;
        Debug.Log("StartSecondPawningCycle");
        ratioOfPawn = 2;
        customersToSpawn = 6;
        customersSpawned = 0;
    }

    public void StartThidPawningCycle()
    {
        isSpawning = true;
        Debug.Log("StartThidPawningCycle");
        ratioOfPawn = 1;
        customersToSpawn = 10;
        customersSpawned = 0;

    }

    private void StopSpawning()
    {
        Debug.Log("StopSpawning");
        isSpawning = false;
    }

    private void OnRestaurantGotFull()
    {
        enabledToSpawn = false;
    }

    private void OnRestaurantGotFree()
    {
        enabledToSpawn = true;
    }

    private void OnCustomerLeft(CustomerBehaviour customer)
    {
        Destroy(customer.gameObject);
    }
}
