using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class LookAtPlayerAction : GoapAction
{
    GoapAI MyAI;

    protected override void Awake()
    {
        myAgent = GetComponent<GoapAgent>();
        myAgent.WorldStateChecks.Add(CheckWorldState);
        MyAI = GetComponent<GoapAI>();
        RequiredStates.Add(new GoapState("In Weapon Range", true));
        SatisfiesStates.Add(new GoapState("Looking At Player", true));
    }

    protected override void CheckWorldState()
    {
        if (myAgent.CurrentWorldState.Find(x => x.Name == "Looking At Player") != null)
        {
            if (GetLookAngle() < MyAI.LookAccuracy)
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "Looking At Player").Status = true;
            }
            else
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "Looking At Player").Status = false;
            }
        }
    }

    public override void BeginAction()
    {
        
    }

    public override bool CanActionRun()
    {
        return PlayerController.PC != null && MyAI != null;
    }

    public override void EndAction()
    {
        isComplete = true;        
    }

    public override bool HasActionFinished()
    {
        return isComplete;
    }

    public override void Reset()
    {

    }

    public override bool RunAction()
    {
        Vector3 PlayerVec = PlayerController.PC.transform.position - transform.position;
        Vector3 ForwardVec = transform.forward;
        PlayerVec.Scale(new Vector3(1, 0, 1));
        ForwardVec.Scale(new Vector3(1, 0, 1));

        float angle = Vector3.Angle(PlayerVec, ForwardVec);
        float timeMod = 120f / angle;
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.FromToRotation(ForwardVec, PlayerVec), Time.deltaTime * timeMod);

        if (GetLookAngle() < MyAI.LookAccuracy)
        {
            isComplete = true;
        }
        return true;
    }

    float GetLookAngle()
    {
        Vector3 PlayerVec = PlayerController.PC.transform.position - transform.position;
        Vector3 ForwardVec = transform.forward;
        PlayerVec.Scale(new Vector3(1, 0, 1));
        ForwardVec.Scale(new Vector3(1, 0, 1));
        return Vector3.Angle(PlayerVec, ForwardVec);
    }

    public override void StopAction()
    {
        
    }

}
