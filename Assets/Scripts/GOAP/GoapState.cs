using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoapState
{
    // The name of the state.
    public string Name;
    // The status of the state.
    public object Status;

    public GoapState()
    {
        Name = "";
        Status = null;
    }

    public GoapState(string name, object status)
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
        return (a.Name == b.Name && a.Status == b.Status);
    }
}
