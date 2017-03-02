using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GetPatrolNodeAction : GoapAction
{
    GoToPatrolNodeAction myGoTo;

    protected override void Awake()
    {
        myAgent = GetComponent<GoapAgent>();
        myAgent.WorldStateChecks.Add(CheckWorldState);
        SatisfiesStates.Add(new GoapState("Has Patrol Node", true));
        myGoTo = GetComponent<GoToPatrolNodeAction>();
    }

    protected override void CheckWorldState()
    {
        if(myGoTo != null)
        {
            if(myGoTo.Target != null)
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "Has Patrol Node").Status = true;
            }
            else
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "Has Patrol Node").Status = false;
            }
        }
        else
        {
            myAgent.CurrentWorldState.Find(x => x.Name == "Has Patrol Node").Status = false;
        }
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
        myGoTo.SetTarget(GetPatrolPoint());
        EndAction();
        return true;
    }

    public override void EndAction()
    {
        isComplete = true;
        
    }

    Vector3 GetPatrolPoint()
    {
        return new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
    }

    public override void Reset()
    {

    }

    public override void StopAction()
    {
        
    }

}
