// core GOAP functionality based on work from: http://www.gdcvault.com/play/1022019/Goal-Oriented-Action-Planning-Ten

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoapState
{
    // The name of the state.
    public string Name;
    // The status of the state.
    public bool Status;

    public GoapState()
    {
        Name = "";
        Status = false;
    }

    public GoapState(string name, bool status)
    {
        Name = name;
        Status = status;
    }

    public bool Compare(GoapState target)
    {
        return (Name == target.Name && Status == target.Status);
    }

    public static bool Compare(GoapState a, GoapState b)
    {
        //Debug.Log("names: (" + a.Name + ", " + b.Name + ") " + (a.Name == b.Name) + " | Status: (" + a.Status + ", " + b.Status + ") " + (a.Status == b.Status) + " | Combined: " + (a.Name == b.Name && a.Status == b.Status));
        return (a.Name == b.Name && a.Status == b.Status);
    }
}
