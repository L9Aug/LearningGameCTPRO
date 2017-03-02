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
        BoolCondition IsDeadCond = new BoolCondition(IsDead);
        //BoolCondition IsAlerted = new BoolCondition();
        //BoolCondition IsNotAlerted = new BoolCondition();
        // Transitions
        Transition Dying = new Transition("Dying", IsDeadCond);
        //Transition AlertedTrans = new Transition("Alerted", IsAlerted);
        //Transition UnAlerted = new Transition("UnAlerted", IsNotAlerted);

        // States
        State Alive = new State("Alive",
            new List<Transition>() { Dying, /*AlertedTrans*/ },
            new List<Action>() { },
            new List<Action>() { AliveUpdate },
            new List<Action>() { EndAlive } );

        State Alerted = new State("Alerted",
            new List<Transition>() { Dying, /*UnAlerted*/ },
            new List<Action>() { },
            new List<Action>() { },
            new List<Action>() { });

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
