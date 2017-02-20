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

    }

}
