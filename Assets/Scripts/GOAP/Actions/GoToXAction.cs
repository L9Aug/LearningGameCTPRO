using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;


public class GoToXAction : GoapAction {

    NavMeshAgent agent;

    public float GetInRange = 0;

    Vector3 TargetDest;

    protected override void Reset()
    {

    }

    protected override void BeginAction()
    {
        agent = GetComponent<NavMeshAgent>();

        if(Target is Transform)
        {
            TargetDest = ((Transform)Target).position;
        }
        else if (Target is Vector3)
        {
            TargetDest = (Vector3)Target;
        }

        agent.SetDestination(TargetDest);
    }

    protected override bool RunAction()
    {
        if(Target == null)
        {
            return false;
        }

        if (Target is Transform)
        {
            if(TargetDest != ((Transform)Target).position)
            {
                TargetDest = ((Transform)Target).position;
                agent.SetDestination(TargetDest);
            }
        }
        else if (Target is Vector3)
        {
            if(TargetDest != (Vector3)Target)
            {
                TargetDest = (Vector3)Target;
            }
        }

        if (agent.remainingDistance <= GetInRange)
        {
            isComplete = true;
        }
        return true;
    }

    protected override bool HasActionFinished()
    {
        return isComplete;
    }

}