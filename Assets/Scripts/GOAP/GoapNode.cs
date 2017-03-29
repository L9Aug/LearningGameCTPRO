// core GOAP functionality based on work from: http://www.gdcvault.com/play/1022019/Goal-Oriented-Action-Planning-Ten

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{

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

}