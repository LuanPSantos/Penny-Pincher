using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.AI;

public class GameManager : MonoBehaviour
{

    public static event Action GameStarted;
    public static event Action GameEnded;
    public static event Action FistPeriodStarted;
    public static event Action FistPeriodEnded;
    public static event Action SecondPeriodStarted;
    public static event Action SecondPeriodEnded;
    public static event Action ThirdPeriodStarted;
    public static event Action ThirdPeriodEnded;

    private List<Action> periodEvents = new List<Action>();

    private int indexPeriod = 0;
    private State state = State.BEGINING;

    private int beginingTime = 5;
    private float timer = 0;

    void OnEnable()
    {
        GameStarted += OnGameStarted;
        ThirdPeriodEnded += OnThirdPeriodEnded;
        SpawnManager.AllCustomerSpawned += OnAllCustomerSpawned;
    }

    void OnDisable()
    {
        GameStarted -= OnGameStarted;
        ThirdPeriodEnded += OnThirdPeriodEnded;
        SpawnManager.AllCustomerSpawned -= OnAllCustomerSpawned;
    }

    void Awake()
    {
        periodEvents.Add(FistPeriodStarted);
        periodEvents.Add(FistPeriodEnded);
        periodEvents.Add(SecondPeriodStarted);
        periodEvents.Add(SecondPeriodEnded);
        periodEvents.Add(ThirdPeriodStarted);
        periodEvents.Add(ThirdPeriodEnded);
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        switch(state)
        {
            case State.BEGINING:
                OnBeginning();
                break;
            case State.PLAYING:
                OnPlaying();
                break;
            case State.END:
                OnEnd();
                break;
            default: break;
        }
    }

    void StartGame()
    {
        GameStarted?.Invoke();
    }

    private void OnGameStarted()
    {
        state = State.BEGINING;
    }

    private void OnAllCustomerSpawned()
    {
        DispatchCurrentPeriodEvent();
    }

    private void DispatchCurrentPeriodEvent()
    {
        periodEvents[indexPeriod]?.Invoke();
        indexPeriod++;
    }

    private void OnBeginning()
    {
        timer += Time.deltaTime;

        if(timer > beginingTime)
        {
            timer = 0;
            DispatchCurrentPeriodEvent();
            state = State.PLAYING;
        }
    }

    private void OnThirdPeriodEnded()
    {
        state = State.END;
    }

    private void OnPlaying()
    {

    }

    private void OnEnd()
    {

    }

    enum State
    {
        BEGINING, PLAYING, END
    }
}
