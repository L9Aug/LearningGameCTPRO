using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

namespace GOAP
{

    public class GoapAgent : MonoBehaviour
    {
        public GoapPlanner myPlanner;

        public List<GoapState> CurrentWorldState = new List<GoapState>();

        public List<GoapGoal> Goals = new List<GoapGoal>();

        public GoapGoal CurrentGoal;

        public StateMachine AgentStateMachine;

        public Queue<GoapAction> ActionPlan = new Queue<GoapAction>();

        #region States

        #endregion

        public void GetNewPlan()
        {
            ClearPlan();
            ActionPlan = myPlanner.GoapPlan(this);
        }

        void ClearPlan()
        {
            while (ActionPlan.Count > 0)
            {
                Destroy(ActionPlan.Dequeue());
            }
        }

    }

}