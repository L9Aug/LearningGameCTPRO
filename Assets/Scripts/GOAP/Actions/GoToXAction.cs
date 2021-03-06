﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;


public class GoToXAction : GoapAction
{

    protected NavMeshAgent agent;
    protected Vector3 TargetDest;

    public float GetInRange = 0;

    protected override void Awake() { }

    protected override void CheckWorldState() { }

    public override void Reset()
    {
        if (agent != null)
        {
            if (agent.isOnNavMesh)
            {
                agent.Resume();
            }
        }
        isComplete = false;
    }

    public override bool CanActionRun()
    {
        return true;
    }

    public override void BeginAction()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.Resume();

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

    public override bool RunAction()
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
            EndAction();
        }

        return true;
    }

    public override void EndAction()
    {
        isComplete = true;
        StopAction();
    }

    public override bool HasActionFinished()
    {
        return isComplete;
    }

    public override void StopAction()
    {
        if (agent != null)
        {
            if (agent.isOnNavMesh)
            {
                agent.Stop();
            }
        }
    }

}