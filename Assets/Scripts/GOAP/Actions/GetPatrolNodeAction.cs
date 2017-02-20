using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class GetPatrolNodeAction : GoapAction
{
    GoToPatrolNodeAction myGoTo;

    protected override void Awake()
    {
        SatisfiesStates.Add(new GoapState("Has Patrol Node", true));
        myGoTo = GetComponent<GoToPatrolNodeAction>();
    }

    public override bool CanActionRun()
    {
        if(myGoTo == null)
        {
            myGoTo = GetComponent<GoToPatrolNodeAction>();
        }

        return (myGoTo != null);
    }

    public override void BeginAction()
    {
        
    }

    public override bool HasActionFinished()
    {
        return isComplete;
    }

    public override bool RunAction()
    {

        return true;
    }

    public override void Reset()
    {
        isComplete = false;
    }

}
