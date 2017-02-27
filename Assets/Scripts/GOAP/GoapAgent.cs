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

    public void Initialise()
    {
        // get actions
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
        CurrentAction = (ActionPlan != null ? (ActionPlan.Count > 0 ? ActionPlan.Dequeue() : null) : null);
        if(CurrentAction != null)
        {
            CurrentAction.BeginAction();
        }
        else
        {
            if(GOAPPlanController.PC != null) GOAPPlanController.PC.RequestPlan(this);
            //GetNewPlan();
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
                        print("No path to goal found");
                    }
                }
                else
                {
                    // plan found
                    FoundGoalPath = true;
                    print("Found path to goal.");
                }

            }
        }
    }

    void ClearPlan()
    {
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

}

