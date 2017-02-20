using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class IdleGoal : GoapGoal
{

    public IdleGoal() : base()
    {
        GoalName = "Idle";
    }

    public override void AddWorldStates(ref List<GoapState> CWS)
    {
        //throw new NotImplementedException();
    }

    public override void SetupUtilityAction()
    {
        UtilityConsideration nConsideration = new UtilityConsideration();
        nConsideration.GetInput = () => { return 0; };

        myUtilityAction = new UtilityAction<GoapGoal>(1, this, nConsideration);
    }

}
