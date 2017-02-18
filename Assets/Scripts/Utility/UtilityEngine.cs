using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [System.Serializable]
    public class UtilityEngine
    {
        public List<UtilityAction> Actions = new List<UtilityAction>();

        object RunUtilityEngine()
        {
            // Only run if we have actions
            if (Actions != null)
            {
                UtilityAction highestScore = null;

                // Loop through every action that we have.
                foreach (UtilityAction action in Actions)
                {
                    // for each action calculate it's compensation value to even out results for actions that have a lot of considerations
                    float CompensationValue = (action.Considerations.Count > 0) ? 1 - (1 / action.Considerations.Count) : 0;
                    float ActionScore = 1;

                    if (action.Considerations.Count > 0)
                    {
                        // Loop throug this actions considerations
                        foreach (UtilityConsideration consideration in action.Considerations)
                        {
                            // Get the score for this consideration.
                            float tempScore = consideration.GetScore();
                            // Get a modified score from our compensation
                            float ModificationValue = (1 - tempScore) * CompensationValue;
                            // Compensate the score we are given
                            tempScore += tempScore * ModificationValue;
                            // Multiply our cumulative score by our compensated score.
                            ActionScore *= tempScore;
                        }

                        // take the actions weight into account
                        ActionScore *= action.Weight;
                    }
                    else
                    {
                        // If the action doesn't have considerations set it's score to 0
                        ActionScore = 0;
                    }                    

                    action.Score = ActionScore;

                    // Test to see if this action has the highest score of all actions so far.
                    if (highestScore != null)
                    {
                        if (highestScore.Score < ActionScore)
                        {
                            highestScore = action;
                        }
                    }
                    else
                    {
                        highestScore = action;
                    }
                }

                // Return the object linked with the action with the highest score.
                return (highestScore != null) ? highestScore.ObjectReference : Actions[0].ObjectReference;
            }
            Debug.LogWarning("No Actions");
            return null;
        }
    }

}