using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CoverGoal : GoapGoal
{
    Health MyHealth;

    public CoverGoal(GoapAgent agent) : base(agent)
    {
        GoalName = "Cover Goal";
        RequiredWorldState.Add(new GoapState("In Cover", true));
        MyHealth = agent.GetComponent<Health>();
    }

    public override void AddWorldStates(ref List<GoapState> CWS)
    {
        CWS.Add(new GoapState("In Cover", false));
    }

    public override void SetupUtilityAction()
    {
        UtilityConsideration LowHeathCons = new UtilityConsideration(UtilityConsideration.CurveTypes.Polynomial, new Vector2(0, MyAgent.GetComponent<Health>().MaxHealth),
            GetHealth, -1, 0.6f, 0, 2, 1);
        myUtilityAction = new UtilityAction<GoapGoal>(1, this, LowHeathCons);
    }

    float GetHealth()
    {
        return MyHealth.health;
    }

}
