﻿using System.Collections;
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

    void ProgressGoap()
    {
        
    }

    void SetupStateMachine()
    {

    }
        
}
