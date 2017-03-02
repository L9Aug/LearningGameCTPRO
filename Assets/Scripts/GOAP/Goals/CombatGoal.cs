using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CombatGoal : GoapGoal
{
    GoapAI myAI;

    public CombatGoal(GoapAgent agent) : base(agent)
    {
        GoalName = "Combat Goal";

        RequiredWorldState.Add(new GoapState("Player Detected", true));
        RequiredWorldState.Add(new GoapState("Shoot At Player", true));

        myAI = agent.GetComponent<GoapAI>();        
    }

    public override void AddWorldStates(ref List<GoapState> CWS)
    {
        if (CWS.Find(x => x.Name == "Player Detected") == null)
        {
            CWS.Add(new GoapState("Player Detected", false));
        }

        if (CWS.Find(x => x.Name == "Shoot At Player") == null)
        {
            CWS.Add(new GoapState("Shoot At Player", false));
        }

        if (CWS.Find(x => x.Name == "Aimed At Player") == null)
        {
            CWS.Add(new GoapState("Aimed At Player", false));
        }

        if (CWS.Find(x => x.Name == "In Weapon Range") == null)
        {
            CWS.Add(new GoapState("In Weapon Range", false));
        }

        if (CWS.Find(x => x.Name == "Looking At Player") == null)
        {
            CWS.Add(new GoapState("Looking At Player", false));
        }

    }

    public override void SetupUtilityAction()
    {
        UtilityConsideration PlayerDetectedCons = new UtilityConsideration();
        PlayerDetectedCons.GetInput = GetConcInput;

        UtilityConsideration PlayerRangeCons = new UtilityConsideration(UtilityConsideration.CurveTypes.Trigonometric, new Vector2(0, MyAgent.GetComponent<GoapAI>().DetectionRadius),
            GetPlayerDist, 1, 1, 2, 1, 0);

        myUtilityAction = new UtilityAction<GoapGoal>(1, this, PlayerDetectedCons, PlayerRangeCons);
    }

    float GetPlayerDist()
    {
        return  Vector3.Distance(MyAgent.transform.position, PlayerController.PC.transform.position);
    }

    float GetConcInput()
    {
        return (myAI.isAlerted) ? 1 : 0;
    }

}
