using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GetInWeaponRangeAction : GoToXAction
{

    BaseWeapon MyWeapon;

    protected override void Awake()
    {
        myAgent = GetComponent<GoapAgent>();
        myAgent.WorldStateChecks.Add(CheckWorldState);
        MyWeapon = GetComponent<GoapAI>().MyGun;
        //RequiredStates.Add(new GoapState("In Weapon Range", true));
        SatisfiesStates.Add(new GoapState("In Weapon Range", true));
    }

    protected override void CheckWorldState()
    {

        if (myAgent.CurrentWorldState.Find(x => x.Name == "In Weapon Range") != null)
        {
            float dist = Vector3.Distance(PlayerController.PC.transform.position, transform.position);
            float distToTarget = Vector3.Distance(transform.position, GetTargetPos());
            if (dist < MyWeapon.WeaponRange || distToTarget <= GetInRange)
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "In Weapon Range").Status = true;
            }
            else
            {
                myAgent.CurrentWorldState.Find(x => x.Name == "In Weapon Range").Status = false;
            }
        }
    }

    public override void BeginAction()
    {
        Target = GetTargetPos();
        base.BeginAction();
    }

    public override bool CanActionRun()
    {
        if (MyWeapon == null) MyWeapon = GetComponent<GoapAI>().MyGun;
        return PlayerController.PC != null && MyWeapon != null;
    }

    public override bool RunAction()
    {
        if (PlayerController.PC == null || MyWeapon == null) return false;
        Target = GetTargetPos();

        if (agent.destination != (Vector3)Target)
        {
            //TargetDest = (Vector3)Target;
            agent.SetDestination((Vector3)Target);
        }

        float dist = Vector3.Distance(PlayerController.PC.transform.position, transform.position);

        if (dist < MyWeapon.WeaponRange || agent.remainingDistance <= GetInRange)
        {
            isComplete = true;
        }

        return true;
    }

    Vector3 GetTargetPos()
    {
        Vector3 PlayerVec = transform.position - PlayerController.PC.transform.position;
        return (PlayerVec.normalized * (0.9f * MyWeapon.WeaponRange)) + PlayerController.PC.transform.position;
    }

    public override void EndAction()
    {
        base.EndAction();        
    }

    public override bool HasActionFinished()
    {
        return isComplete;
    }

    public override void Reset()
    {
        base.Reset();
    }


}
