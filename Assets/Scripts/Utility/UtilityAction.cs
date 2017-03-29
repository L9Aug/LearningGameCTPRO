// core utility functionality based on work from: from http://www.gdcvault.com/play/1021848/Building-a-Better-Centaur-AI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{

    [System.Serializable]
    public class UtilityAction<T>
    {
        public List<UtilityConsideration> Considerations = new List<UtilityConsideration>();
        public float Weight;
        public float Score;
        public T ObjectReference;

        public UtilityAction()
        {
            Weight = 1;
        }

        public UtilityAction(float weight, T objectRef, params UtilityConsideration[] considerations)
        {
            Weight = weight;
            ObjectReference = objectRef;
            Considerations.AddRange(considerations);
        }

    }

}
