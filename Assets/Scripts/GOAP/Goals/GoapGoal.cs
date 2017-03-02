using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

[System.Serializable]
public abstract class GoapGoal
{
    public string GoalName;
    public GoapAgent MyAgent;
    public List<GoapState> RequiredWorldState = new List<GoapState>();

    public UtilityAction<GoapGoal> myUtilityAction;

    public abstract void SetupUtilityAction();

    public abstract void AddWorldStates(ref List<GoapState> CWS);

    public GoapGoal(GoapAgent agent)
    {
        MyAgent = agent;
        GoalName = "";
        SetupUtilityAction();
    }

    public GoapGoal(string name, GoapAgent agent)
    {
        MyAgent = agent;
        GoalName = name;
        SetupUtilityAction();
    }

    public GoapGoal(string name, List<GoapState> requiredWorldState, GoapAgent agent)
    {
        MyAgent = agent;
        GoalName = name;
        RequiredWorldState = requiredWorldState;
        SetupUtilityAction();
    }

}
