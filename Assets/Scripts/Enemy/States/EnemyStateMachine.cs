using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    NULL_STATE,
    IDLE,
    CIRCLING,
    ENGAGE,
    ATTACK,
    RETREAT,
    STUNNED,
    SPAWNING,
    DEAD,
    NUM_STATES
}

public class EnemyStateMachine
{
    private EnemyController enemyController;
    private Dictionary<EnemyStates, EnemyStateBase<EnemyStates>> KeyToEnemyStates = new Dictionary<EnemyStates, EnemyStateBase<EnemyStates>>();
    EnemyStates ActiveState = EnemyStates.NULL_STATE;

    public void AddState<NewState>() where NewState : EnemyStateBase<EnemyStates>, new()
    {
        var newstate = new NewState();
        newstate.controller = enemyController;
        newstate.Init();
        KeyToEnemyStates.Add(newstate.key, newstate);
    }

    public void ChangeState(EnemyStates key)
    {
        if (key == ActiveState)
        {
            return;
        }

        EnemyStateBase<EnemyStates> newState;
        if (!KeyToEnemyStates.TryGetValue(key, out newState))
        {
            Debug.LogError("State " + key.ToString() + " does not exist within state machine!");
            return;
        }

        if (ActiveState != key)
        {
            if (ActiveState != EnemyStates.NULL_STATE)
            {
                EnemyStateBase<EnemyStates> oldState = KeyToEnemyStates[ActiveState];
                oldState.OnExit();
            }
            ActiveState = key;
        }

        newState.OnEnter();
    }

    public void Init(EnemyController controller)
    {
        enemyController = controller;
    }

    public void Update()
    {
        if (ActiveState == EnemyStates.NULL_STATE)
        {
            return;
        }
        EnemyStateBase<EnemyStates> currentState = KeyToEnemyStates[ActiveState];
        ChangeState(currentState.OnUpdate());
    }
}
