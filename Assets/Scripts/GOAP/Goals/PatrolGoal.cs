using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PatrolGoal : GoapGoal
{

    public PatrolGoal(GoapAgent agent) : base(agent)
    {
        GoalName = "PatrolGoal";

        RequiredWorldState.Add(new GoapState("At Patrol Node", true));
        //RequiredWorldState.Add(new GoapState("Has Patrol Node", true));
    }

    public override void SetupUtilityAction()
    {
        UtilityConsideration nConsideration = new UtilityConsideration();
        nConsideration.GetInput = () => { return 0.1f; };

        myUtilityAction = new UtilityAction<GoapGoal>(1, this, nConsideration);
    }

    public override void AddWorldStates(ref List<GoapState> CWS)
    {
        if(CWS.Find(x => x.Name == "At Patrol Node") == null)
        {
            CWS.Add(new GoapState("At Patrol Node", false));
        }

        if(CWS.Find(x => x.Name == "Has Patrol Node") == null)
        {
            CWS.Add(new GoapState("Has Patrol Node", false));
        }
    }

}
