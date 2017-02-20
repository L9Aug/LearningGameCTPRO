using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Utility;

namespace GOAP
{

    public class GoapAgent : MonoBehaviour
    {
        public GoapPlanner myPlanner;

        public UtilityEngine<GoapGoal> myUtilityEngine = new UtilityEngine<GoapGoal>();

        public List<GoapState> CurrentWorldState = new List<GoapState>();

        public List<GoapGoal> Goals = new List<GoapGoal>();

        public GoapGoal CurrentGoal;

        public GoapAction CurrentAction;

        public StateMachine AgentStateMachine;

        public Queue<GoapAction> ActionPlan = new Queue<GoapAction>();

        #region States

        #endregion

        public void RunPlan()
        {
            if (CurrentAction != null)
            {
                CurrentAction.RunAction();

                if (CurrentAction.HasActionFinished())
                {
                    // get next action
                    CurrentAction = ActionPlan.Dequeue();
                    if(CurrentAction == null)
                    {
                        //get new plan
                        GetNewPlan();
                    }
                }

            }
            else
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

        public void GetNewPlan()
        {
            ClearPlan();

            CurrentGoal = myUtilityEngine.RunUtilityEngine();

            ActionPlan = myPlanner.GoapPlan(this);
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
            }
        }

    }

}