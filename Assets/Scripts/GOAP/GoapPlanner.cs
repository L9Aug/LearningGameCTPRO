using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{

    public class GoapPlanner
    {
        public List<GoapState> CurrentWorldState;
        public List<GoapAction> AvailableActions;

        public Queue<GoapAction> GoapPlan(GoapAgent agent)
        {
            CurrentWorldState = agent.CurrentWorldState;
            AvailableActions = agent.AvailableActions;

            //get an action tree for the current goal
            List<GoapNode> GoalTrees = new List<GoapNode>();
            GoalTrees.AddRange(GetGoalTree(agent.CurrentGoal));

            // If there is at least one path return the fastest path, otherwise return that no path was found.
            if (GoalTrees.Count > 0)
            {
                // search all trees for the fastest path.
                GoalTrees.Sort((x, y) => x.CumulativeCost.CompareTo(y.CumulativeCost));
                return ProccessNodeListIntoQueue(GoalTrees[0]);
            }
            else
            {
                // No path found...
                Debug.Log("No GOAP path found for " + agent.gameObject.name + ". Requested Goal: " + agent.CurrentGoal);
                return null;
            }
        }

        // takes the node path that is passed in and turns it into a queue of actions.
        Queue<GoapAction> ProccessNodeListIntoQueue(GoapNode StartNode)
        {
            Queue<GoapAction> ReturnQueue = new Queue<GoapAction>();
            GoapNode tempNode = StartNode;
            while (tempNode.Parent != null)
            {
                ReturnQueue.Enqueue(tempNode.Action);
                tempNode = tempNode.Parent;
            }
            return ReturnQueue;
        }

        /// <summary>
        /// Gets the possible action branches that arrive at the goals required state.
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        List<GoapNode> GetGoalTree(GoapGoal goal)
        {
            List<GoapNode> EndNodes = new List<GoapNode>();
            GoapNode RootNode = new GoapNode(null, 0, goal.RequiredWorldState, null);

            // check if the world state already matches this goal.
            if (TestGoalStateAgainstWorldState(goal)) return new List<GoapNode>();

            List<GoapAction> ImmedeateActions = GetGoalActions(goal);
            foreach (GoapAction action in ImmedeateActions)
            {
                // test each immediate action's pre-requisite states against the world states.
                if (TestActionAgainstWorldState(action))
                {
                    EndNodes.Add(new GoapNode(RootNode, RootNode.CumulativeCost + action.Cost, action.SatisfiesStates, action));
                }
                else
                {
                    // recurse through this action's state requirements until it either matches the world state or has no prerequisites.
                    EndNodes.AddRange(GetActionTree(action, new GoapNode(RootNode, RootNode.CumulativeCost + action.Cost, action.SatisfiesStates, action)));
                }
            }

            return EndNodes;
        }

        List<GoapNode> GetActionTree(GoapAction action, GoapNode RootNode)
        {
            List<GoapNode> returnList = new List<GoapNode>();
            List<GoapAction> ChildActions = PreCursorActions(action);

            // if this action has pre-requisite requirements that are not fulfilled, then find the actions that satisfy the required worls states.
            if (ChildActions.Count > 0)
            {
                foreach (GoapAction ChildAction in ChildActions)
                {
                    returnList.AddRange(GetActionTree(ChildAction, new GoapNode(RootNode, RootNode.CumulativeCost + action.Cost, action.SatisfiesStates, action)));
                }
            }
            else
            {
                // otherwise if the action has no pre-requisite states return this action.
                if (action.RequiredStates.Count == 0)
                {
                    returnList.Add(new GoapNode(RootNode, RootNode.CumulativeCost + action.Cost, action.SatisfiesStates, action));
                }
            }
            return returnList;
        }

        List<GoapAction> PreCursorActions(GoapAction newAction)
        {
            List<GoapAction> ReturnActions = new List<GoapAction>();
            // loop through this actions required states
            foreach (GoapState state in newAction.RequiredStates)
            {
                // loop through the actions available.
                foreach (GoapAction action in AvailableActions)
                {
                    if (action.CanActionRun())
                    {
                        for (int i = 0; i < action.SatisfiesStates.Count; ++i)
                        {
                            // if one of the actions that is available satisfies one or many of our actions required states then add it to be returned.
                            if (action.SatisfiesStates[i].Compare(state))
                            {
                                ReturnActions.Add(action);
                            }
                        }
                    }
                }
            }
            return ReturnActions;
        }

        // test to see if the actions required states match our current world states.
        bool TestActionAgainstWorldState(GoapAction action)
        {
            if (action.RequiredStates.Count > 0)
            {
                foreach (GoapState actionState in action.RequiredStates)
                {
                    bool currentStateCheck = false;
                    foreach (GoapState worldState in CurrentWorldState)
                    {
                        if (GoapState.Compare(actionState, worldState))
                        {
                            currentStateCheck = true;
                        }
                    }
                    if (!currentStateCheck) return false;
                }
            }
            return true;
        }

        // Returns a list of actions (from the available actions) that satisfy one or all of our goals required states.
        List<GoapAction> GetGoalActions(GoapGoal goal)
        {
            List<GoapAction> ReturnActions = new List<GoapAction>();
            foreach (GoapState state in goal.RequiredWorldState)
            {
                foreach (GoapAction action in AvailableActions)
                {
                    if (action.CanActionRun())
                    {
                        for (int i = 0; i < action.SatisfiesStates.Count; ++i)
                        {
                            if (GoapState.Compare(state, action.SatisfiesStates[i]))
                            {
                                ReturnActions.Add(action);
                            }
                        }
                    }
                }
            }
            return ReturnActions;
        }

        // test to see if our current world state matches that of our goals required states.
        bool TestGoalStateAgainstWorldState(GoapGoal goal)
        {
            bool GoalStatesMatch = true;
            foreach (GoapState goalState in goal.RequiredWorldState)
            {
                bool currentStateCheck = false;
                foreach (GoapState worldState in CurrentWorldState)
                {
                    if (GoapState.Compare(worldState, goalState))
                    {
                        currentStateCheck = true;
                    }
                }

                if (!currentStateCheck)
                {
                    GoalStatesMatch = false;
                    break;
                }
            }
            return GoalStatesMatch;
        }

    }

}