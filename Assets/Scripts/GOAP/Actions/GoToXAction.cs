using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToXAction : GoapAction {

    NavMeshAgent agent;

    protected override void Reset()
    {

    }

    protected override void BeginAction()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override bool RunAction()
    {
        
        return true;
    }

    protected override bool HasActionFinished()
    {
        return isComplete;
    }

}