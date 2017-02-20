using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;

[System.Serializable]
public abstract class GoapGoal
{
    public string GoalName;
    public List<GoapState> RequiredWorldState = new List<GoapState>();

    public UtilityAction<GoapGoal> myUtilityAction;

    public abstract void SetupUtilityAction();

    public GoapGoal()
    {
        GoalName = "";
        SetupUtilityAction();
    }

    public GoapGoal(string name)
    {
        GoalName = name;
        SetupUtilityAction();
    }

    public GoapGoal(string name, List<GoapState> requiredWorldState)
    {
        GoalName = name;
        RequiredWorldState = requiredWorldState;
        SetupUtilityAction();
    }

}
