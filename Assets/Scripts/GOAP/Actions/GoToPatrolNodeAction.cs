using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GoToPatrolNodeAction : GoToXAction
{
    protected override void Awake()
    {
        myAgent = GetComponent<GoapAgent>();
        myAgent.WorldStateChecks.Add(CheckWorldState);
        RequiredStates.Add(new GoapState("Has Patrol Node", true));
        SatisfiesStates.Add(new GoapState("At Patrol Node", true));
    }

    protected override void CheckWorldState()
    {
        if (agent != null)
        {
            if (Target != null)
            {
                Vector3 targetPos = Vector3.zero;
                if (Target is Transform)
                {
                    targetPos = ((Transform)Target).position;
                }
                if (Target is Vector3)
                {
                    targetPos = ((Vector3)Target);
                }

                if(Vector3.Distance(targetPos, transform.position) < GetInRange)
                {
                    myAgent.CurrentWorldState.Find(x => x.Name == "At Patrol Node").Status = true;
                }
                else
                {
                    myAgent.CurrentWorldState.Find(x => x.Name == "At Patrol Node").Status = false;
                }
            }
            else
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "At Patrol Node").Status = false;
            }
        }
        else
        {
            myAgent.CurrentWorldState.Find(x => x.Name == "At Patrol Node").Status = false;
        }
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void EndAction()
    {
        StopAction();
        isComplete = true;
    }

}
