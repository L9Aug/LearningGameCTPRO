using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [System.Serializable]
    public class UtilityEngine<T>
    {
        public List<UtilityAction<T>> Actions = new List<UtilityAction<T>>();

        public List<T> RunUtilityEngine()
        {
            List<UtilityAction<T>> SortedActions = new List<UtilityAction<T>>();

            // Only run if we have actions
            if (Actions.Count > 0)
            {
                //UtilityAction<T> highestScore = null;

                // Loop through every action that we have.
                foreach (UtilityAction<T> action in Actions)
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
                    SortedActions.Add(action);
                }

                // sort the list so that the highest score is at element 0
                // multiplied by 100 as scores are in the range 0-1 before weighting values.
                SortedActions.Sort((x, y) => (int)((x.Score - y.Score) * 100f));

            }
            else
            {
                Debug.LogWarning("No Actions");
            }

            //return SortedActions;
            return ConvertFromActionList(SortedActions);
        }

        List<T> ConvertFromActionList(List<UtilityAction<T>> list)
        {
            List<T> retList = new List<T>();
            for(int i = 0; i < list.Count; ++i)
            {
                retList.Add(list[i].ObjectReference);
            }
            return retList;
        }

    }

}