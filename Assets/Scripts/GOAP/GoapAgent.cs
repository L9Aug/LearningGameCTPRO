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

    public List<GoapState> CurrentWorldState = new List<GoapState>();

    public List<GoapGoal> Goals = new List<GoapGoal>();

    public Queue<GoapAction> ActionPlan = new Queue<GoapAction>();

    public void RunPlan()
    {
        if (CurrentAction != null)
        {
            CurrentAction.RunAction();

            if (CurrentAction.HasActionFinished())
            {
                // get next action
                CurrentAction = ActionPlan.Dequeue();
                if (CurrentAction == null)
                {
                    //get new plan
                    GetNewPlan();
                }
            }
        }
        else
        {
            // get next action
            CurrentAction = (ActionPlan.Count > 0 ? ActionPlan.Dequeue() : null);
            if (CurrentAction == null)
            {
                //get new plan
                GetNewPlan();
            }
        }
    }

    public void GetNewPlan()
    {
        ClearPlan();

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
                    }
                }
                else
                {
                    // plan found
                    FoundGoalPath = true;
                }

            }
        }
    }

    void ClearPlan()
    {
        while (ActionPlan.Count > 0)
        {
            Destroy(ActionPlan.Dequeue());
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

