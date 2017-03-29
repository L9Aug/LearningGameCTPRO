// core GOAP functionality based on work from: http://www.gdcvault.com/play/1022019/Goal-Oriented-Action-Planning-Ten

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Utility;
using GOAP;

public class GoapAgent : MonoBehaviour
{
    public GoapPlanner myPlanner;

    public GoapGoal CurrentGoal;

    public GoapAction CurrentAction;

    public StateMachine AgentStateMachine;

    public UtilityEngine<GoapGoal> myUtilityEngine = new UtilityEngine<GoapGoal>();

    public List<GoapAction> AvailableActions = new List<GoapAction>();

    public List<GoapState> CurrentWorldState = new List<GoapState>();

    public List<GoapGoal> Goals = new List<GoapGoal>();

    public Queue<GoapAction> ActionPlan = new Queue<GoapAction>();

    public delegate void WorldStateCheck();

    public List<WorldStateCheck> WorldStateChecks = new List<WorldStateCheck>();

    public void Initialise()
    {
        // get actions
        AvailableActions.Clear();
        AvailableActions.AddRange(GetComponents<GoapAction>());
        SetupUtilityEngine();
        myPlanner = new GoapPlanner();
    }

    public void RunPlan()
    {
        if (CurrentAction != null)
        {
            CurrentAction.RunAction();

            if (CurrentAction.HasActionFinished())
            {
                // get next action
                GetNextAction();
            }
        }
        else
        {
            // get next action
            GetNextAction();
        }
    }

    void GetNextAction()
    {
        if (CurrentAction != null) CurrentAction.EndAction();
        CurrentAction = (ActionPlan != null ? (ActionPlan.Count > 0 ? ActionPlan.Dequeue() : null) : null);
        if(CurrentAction != null)
        {
            CurrentAction.BeginAction();
        }
        else
        {
            if(GOAPPlanController.PC != null) GOAPPlanController.PC.RequestPlan(this);
        }
    }

    public void GetNewPlan()
    {
        ClearPlan();

        foreach(GoapAction action in AvailableActions)
        {
            action.ResetAction();
        }

        List<GoapGoal> orderedGoals = myUtilityEngine.RunUtilityEngine();
        UpdateWorldState();
        if (orderedGoals.Count > 0)
        {
            bool FoundGoalPath = false;
            int i = 0;
            while (FoundGoalPath == false)
            {
                CurrentGoal = orderedGoals[i];
                ActionPlan = myPlanner.GoapPlan(this);
                if (ActionPlan == null)
                {
                    // no plan found.
                    ++i;
                    if (i >= orderedGoals.Count)
                    {
                        // end the loop if no path can be found to any goal.
                        FoundGoalPath = true;
                    }
                }
                else
                {
                    // plan found
                    GetNextAction();
                    FoundGoalPath = true;
                }

            }
        }
    }

    public void UpdateWorldState()
    {
        for(int i = 0; i < WorldStateChecks.Count; ++i)
        {
            if(WorldStateChecks[i] != null)
            {
                WorldStateChecks[i]();
            }
            else
            {
                WorldStateChecks.RemoveAt(i);
                --i;
            }
        }
    }

    public void ClearPlan()
    {
        StopAllActions();
        if (ActionPlan != null)
        {
            ActionPlan.Clear();
        }
    }

    void SetupUtilityEngine()
    {
        // add the utility actions from each goal that we have.
        foreach (GoapGoal goal in Goals)
        {
            myUtilityEngine.Actions.Add(goal.myUtilityAction);
            // add world states.
            goal.AddWorldStates(ref CurrentWorldState);
        }
    }

    public void StopAllActions()
    {
        foreach(GoapAction action in AvailableActions)
        {
            action.StopAction();
        }
    }

}
