using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class GoToPatrolNodeAction : GoToXAction
{
    protected override void Awake()
    {
        RequiredStates.Add(new GoapState("Has Patrol Node", true));
        SatisfiesStates.Add(new GoapState("Is At Patrol Node", true));
    }

}
