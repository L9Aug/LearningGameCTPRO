using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    public bool isComplete;

    public GameObject Target;

    public List<GoapState> SatisfiesStates = new List<GoapState>();
    public List<GoapState> RequiredStates = new List<GoapState>();

    public float Cost = 1;

    /// <summary>
    /// Resets the parameters of this action.
    /// </summary>
    public void ResetAction()
    {
        Reset();
    }

    protected abstract void Reset();

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    /// <summary>
    /// To be called when the action is first run.
    /// </summary>
    protected abstract void BeginAction();

    /// <summary>
    /// Performs the action.
    /// </summary>
    /// <returns></returns>
    protected abstract bool RunAction();

    /// <summary>
    /// Returns whether or not the action is finished.
    /// </summary>
    /// <returns></returns>
    protected abstract bool HasActionFinished();

}
