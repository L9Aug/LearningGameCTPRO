using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoapNode
{
    public GoapNode Parent;
    public List<GoapNode> Children = new List<GoapNode>();
    public float CumulativeCost;
    public List<GoapState> SatisfiedStates;
    public GoapAction Action;

    public GoapNode()
    {
        SatisfiedStates = new List<GoapState>();
    }

    public GoapNode(GoapNode parent, float cumulativeCost, List<GoapState> satisfiedStates, GoapAction action)
    {
        Parent = parent;
        CumulativeCost = cumulativeCost;
        SatisfiedStates = satisfiedStates;
        Action = action;
    }

}
