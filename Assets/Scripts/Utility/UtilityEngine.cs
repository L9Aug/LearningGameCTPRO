using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityEngine : MonoBehaviour
{
    public List<UtilityAction> Actions = new List<UtilityAction>();
    
    object RunUtilityEngine()
    {
        UtilityAction highestScore = null;

        foreach(UtilityAction action in Actions)
        {
            float CompensationValue = (action.Considerations.Count > 0) ? 1 - (1 / action.Considerations.Count) : 0;
            float ActionScore = 1;

            foreach(UtilityConsideration consideration in action.Considerations)
            {
                float tempScore = consideration.GetScore();
                float ModificationValue = (1 - tempScore) * CompensationValue;
                tempScore += tempScore + ModificationValue;
                ActionScore *= tempScore;
            }

            action.Score = ActionScore;

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

        return (highestScore != null) ? highestScore.ObjectReference : Actions[0].ObjectReference;
    }
}
