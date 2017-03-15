using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{

    public abstract class GoapAction : MonoBehaviour
    {
        public bool isComplete;

        public object Target;

        public GoapAgent myAgent;

        public List<GoapState> SatisfiesStates = new List<GoapState>();
        public List<GoapState> RequiredStates = new List<GoapState>();

        public float Cost = 1;

        protected abstract void CheckWorldState();

        protected abstract void Awake();

        protected virtual void Start() { }

        public abstract bool CanActionRun();

        /// <summary>
        /// Resets the parameters of this action.
        /// </summary>
        public void ResetAction()
        {
            Target = null;
            isComplete = false;
            Reset();
        }

        public abstract void Reset();

        public void SetTarget(object target)
        {
            Target = target;
        }

        public abstract void EndAction();

        /// <summary>
        /// To be called when the action is first run.
        /// </summary>
        public abstract void BeginAction();

        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <returns></returns>
        public abstract bool RunAction();

        /// <summary>
        /// Returns whether or not the action is finished.
        /// </summary>
        /// <returns></returns>
        public abstract bool HasActionFinished();

        public abstract void StopAction();

    }

}