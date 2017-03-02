using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class AimAtPlayerAction : GoapAction
{
    BaseWeapon MyWeapon;

    protected override void Awake()
    {
        myAgent = GetComponent<GoapAgent>();
        myAgent.WorldStateChecks.Add(CheckWorldState);
        RequiredStates.Add(new GoapState("Looking At Player", true));
        SatisfiesStates.Add(new GoapState("Aimed At Player", true));
        MyWeapon = GetComponent<GoapAI>().MyGun;
    }

    protected override void CheckWorldState()
    {
        if(MyWeapon != null)
        {
            if (MyWeapon.isAimed)
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "Aimed At Player").Status = true;
            }
            else
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "Aimed At Player").Status = false;
            }
        }
        else
        {
            myAgent.CurrentWorldState.Find(x => x.Name == "Aimed At Player").Status = false;
        }
    }

    public override void BeginAction()
    {

    }

    public override bool CanActionRun()
    {
        if (MyWeapon == null) MyWeapon = GetComponent<GoapAI>().MyGun;
        return PlayerController.PC != null && MyWeapon != null;
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
        if(MyWeapon != null)
        {
            MyWeapon.isAimed = false;
        }
    }

    public override bool RunAction()
    {
        if (MyWeapon == null) return false;

        if (myAgent.GetComponent<GoapAI>().CanAimWeapon) MyWeapon.isAimed = true;
        isComplete = true;
        return true;
    }

    public override void StopAction()
    {
        if (MyWeapon != null)
        {
            MyWeapon.isAimed = false;
        }
    }

}
