﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UtilityAction
{
    public List<UtilityConsideration> Considerations = new List<UtilityConsideration>();
    public float Weight;
    public float Score;
    public object ObjectReference;

}