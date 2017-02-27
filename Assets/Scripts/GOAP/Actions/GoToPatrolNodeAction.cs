using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GoToPatrolNodeAction : GoToXAction
{
    protected override void Awake()
    {
        RequiredStates.Add(new GoapState("Has Patrol Node", true));
        SatisfiesStates.Add(new GoapState("At Patrol Node", true));
    }

    public override void Reset()
    {
        
    }

    public override void EndAction()
    {
        isComplete = true;
        myAgent.CurrentWorldState.Find(x => x.Name == "At Patrol Node").Status = false;
        myAgent.CurrentWorldState.Find(x => x.Name == "Has Patrol Node").Status = false;
    }

}
