using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Utility;
using GOAP;

public class GoapAI : MonoBehaviour, IGoap
{
    StateMachine myStateMachine;    

    GoapAgent myAgent;
    
    void Start()
    {
        myAgent = GetComponent<GoapAgent>();
    }

    void Update()
    {
        ProgressGoap();
    }

    void ProgressGoap()
    {
        //print("run Plan");
        myAgent.RunPlan();
    }
            
}
