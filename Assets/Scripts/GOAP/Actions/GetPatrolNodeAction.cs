using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

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
        myGoTo.SetTarget(GetPatrolPoint());
        EndAction();
        return true;
    }

    public override void EndAction()
    {
        isComplete = true;
        myAgent.CurrentWorldState.Find(x => x.Name == "Has Patrol Node").Status = true;
    }

    Vector3 GetPatrolPoint()
    {
        return new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
    }

    public override void Reset()
    {
        isComplete = false;
    }

    public override void StopAction()
    {
        
    }

}
