using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GetPatrolNodeAction : GoapAction
{
    GoToPatrolNodeAction myGoTo;
    List<Vector3> PatrolPoints = new List<Vector3>();
    int currentPatrolPoint = -1;
    
    protected override void Awake()
    {
        myAgent = GetComponent<GoapAgent>();
        myAgent.WorldStateChecks.Add(CheckWorldState);
        SatisfiesStates.Add(new GoapState("Has Patrol Node", true));
        myGoTo = GetComponent<GoToPatrolNodeAction>();
    }

    protected override void Start()
    {
        int NumPatrolPoints = Random.Range(2, 10);
        float PatrolRadius = 10;
        
        for(int i  = 0; i < NumPatrolPoints; ++i)
        {
            PatrolPoints.Add(Vector3.Scale(Random.insideUnitSphere * PatrolRadius, new Vector3(1, 0, 1)) + transform.position);
        }

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
        ++currentPatrolPoint;
        currentPatrolPoint = currentPatrolPoint % PatrolPoints.Count;
    }

    public override bool HasActionFinished()
    {
        return isComplete;
    }

    public override bool RunAction()
    {
        myGoTo.SetTarget(PatrolPoints[currentPatrolPoint]);
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
