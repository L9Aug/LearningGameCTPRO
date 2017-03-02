using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class ShootAtPlayerAction : GoapAction
{

    BaseWeapon MyWeapon;

    protected override void Awake()
    {
        myAgent = GetComponent<GoapAgent>();
        MyWeapon = GetComponent<GoapAI>().MyGun;

        RequiredStates.Add(new GoapState("Aimed At Player", true));
        SatisfiesStates.Add(new GoapState("Shoot At Player", true));

    }

    protected override void CheckWorldState()
    {

    }

    public override void BeginAction()
    {
        
    }

    public override bool CanActionRun()
    {
        if (MyWeapon == null) MyWeapon = GetComponent<GoapAI>().MyGun;
        return MyWeapon != null;
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
        if (MyWeapon == null) return false;

        if (MyWeapon.Fire(() => { return MyWeapon.transform.position + (MyWeapon.transform.forward * MyWeapon.WeaponRange); }))
        {
            if (MyWeapon.Magazine == 0) MyWeapon.Reload();
            isComplete = true;
        }
        return true;
    }

    public override void StopAction()
    {
        
    }
}
