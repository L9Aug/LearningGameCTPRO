using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Condition;
using Utility;
using GOAP;

public class GoapAI : MonoBehaviour, IGoap
{
    StateMachine myStateMachine;    

    GoapAgent myAgent;

    bool isDead = false;

    void Start()
    {
        myAgent = GetComponent<GoapAgent>();
        GetComponent<Health>().HealthChangedActions.Add(OnHealthChanged);
        SetupStateMachine();
    }

    void Update()
    {
        myStateMachine.SMUpdate();
    }

    void OnHealthChanged(float DeltaHealth, float CurrentHealth, float MaxHealth)
    {
        if(CurrentHealth <= 0)
        {
            isDead = true;
        }
    }

    #region State Machine Functions

    #region Alive State Functions

    void AliveUpdate()
    {
        myAgent.RunPlan();
    }

    void EndAlive()
    {
        myAgent.StopAllActions();
    }

    #endregion

    #region Dead State Function

    #endregion

    #region Conditional Functions

    bool IsDead()
    {
        return isDead;
    }

    #endregion

    void SetupStateMachine()
    {
        // Conditions
        BoolCondition IsDeadCOnd = new BoolCondition(IsDead);

        // Transitions
        Transition Dying = new Transition("Dying", IsDeadCOnd);

        // States
        State Alive = new State("Alive",
            new List<Transition>() { Dying },
            new List<Action>() { },
            new List<Action>() { AliveUpdate },
            new List<Action>() { EndAlive } );

        State Dead = new State("Dead",
            null,
            new List<Action>() { },
            new List<Action>() { },
            null);

        // Target States
        Dying.SetTargetState(Dead);

        // init machine
        myStateMachine = new StateMachine(null, Alive, Dead);
        myStateMachine.InitMachine();
    }

    #endregion
}
