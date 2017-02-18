using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Utility;

public class GoapAI : MonoBehaviour, IGoap
{
    StateMachine myStateMachine;

    UtilityEngine myUtilityEngine;

    GoapAgent myAgent;
    
    void Start()
    {
        myAgent = GetComponent<GoapAgent>();
    }

    void SetupStateMachine()
    {

    }

    void SetupUtilityEngine()
    {
        // add the utility actions from each goal that we have.
        foreach(GoapGoal goal in myAgent.Goals)
        {
            myUtilityEngine.Actions.Add(goal.myUtilityAction);
        }
    }
}
