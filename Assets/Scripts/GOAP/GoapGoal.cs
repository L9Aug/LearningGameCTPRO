using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoapGoal
{
    public string GoalName;
    public List<GoapState> RequiredWorldState = new List<GoapState>();

    GoapGoal()
    {
        GoalName = "";
    }

    GoapGoal(string name)
    {
        GoalName = name;
    }

    GoapGoal(string name, List<GoapState> requiredWorldState)
    {
        GoalName = name;
        RequiredWorldState = requiredWorldState;
    }

}
