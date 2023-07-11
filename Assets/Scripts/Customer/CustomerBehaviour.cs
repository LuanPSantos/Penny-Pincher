using Pathfinding;
using System;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    public static event Action<CustomerBehaviour> Left;

    [SerializeField]
    private float distanceToSit = 1;
    [SerializeField]
    private TMPro.TextMeshPro statusText;

    private TableBehaviour table = null;
    private Transform exitDoor = null;

    private AIDestinationSetter destinationSetter;

    private State state = State.IDLE;

    void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    void OnEnable()
    {
        TableBehaviour.Closed += OnTableClosed;
    }

    void OnDisable()
    {
        TableBehaviour.Closed -= OnTableClosed;
    }

    void Update()
    {
        switch (state)
        {
            case State.GOING_TO_TABLE:
                OnGoingToTable();
                break;
            case State.SITTING:
                OnSitting();
                break;
            case State.LEAVING:
                OnLeaving();
                break;
            default: break;
        }
    }

    public void SetExitDoor(Transform exitDoor)
    {
        this.exitDoor = exitDoor;
    }

    public void SitOnTable(TableBehaviour tableBehaviour)
    {
        table = tableBehaviour;
        state = State.GOING_TO_TABLE;
        statusText.text = state.ToString();
        table.Reserve();
        destinationSetter.target = table.transform;
        Debug.Log(state);
    }

    private void OnGoingToTable()
    {
        if (GotOnTable())
        {
            Sit();
        }
    }

    private void Sit()
    {
        state = State.SITTING;
        statusText.text = state.ToString();
        table.Sit();
    }

    private void LeaveTable()
    {
        state = State.LEAVING;
        statusText.text = state.ToString();
        destinationSetter.target = exitDoor;
    }

    private void OnLeaving()
    {
        var distance = Vector2.Distance(exitDoor.position, transform.position);
        if (distance < 1)
        {
            Left?.Invoke(this);
        }
    }

    private void OnSitting()
    {

    }

    private void OnTableClosed(int tableNumber)
    {
        if(table.tableNumber == tableNumber)
        {
            LeaveTable();
        }
    }

    private bool GotOnTable()
    {
        var distance = Vector2.Distance(table.transform.position, transform.position);
        return distance < distanceToSit;
    }

    enum State
    {
        IDLE, GOING_TO_TABLE, SITTING, LEAVING
    }
}
