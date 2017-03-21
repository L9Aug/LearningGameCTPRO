using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using FSM;
using Condition;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// The player controller
    /// </summary>
    public static PlayerController PC;
    public BaseWeapon CurrentWeapon;
    public Transform PlayerBody;
    public float EquipDist;
    public float EquipRadius;
    public Text EquipText;
    public bool IsDead = false;
    public Camera myCam;

    private StateMachine PSM;
    private StateMachine AimStateMachine;

    private Vector3 TargetPos;
    private Vector3 PreviousPos;
    private float targetFOV;
    private float PreviousFOV;
    float LerpTimer = 0;
    
	void Start ()
    {
	    PC = this;
        GetComponent<Health>().HealthChangedActions.Add(UserInterfaceController.UIC.OnHealthChanged);
        OnWeaponPickup(CurrentWeapon);
        SetupPlayerStateMachine();
	}

    private void Update()
    {
        PSM.SMUpdate();
    }

    void BeginAlive()
    {

    }
    
	void AliveUpdate ()
    {
        CheckWeapon();
        LookForWeapon();
        AimStateMachine.SMUpdate();
	}

    void EndAlive()
    {

    }

    void BeginAim()
    {
        ResetAimVars();
        TargetPos = CurrentWeapon.AimLocation;
        targetFOV = 30;
    }

    void EndAim()
    {
        ResetAimVars();
        TargetPos = CurrentWeapon.EquipLocation[0];
        targetFOV = 60;
    }

    void ResetAimVars()
    {
        LerpTimer = 0;
        PreviousPos = CurrentWeapon.transform.localPosition;
        TargetPos = CurrentWeapon.transform.localPosition;
        PreviousFOV = myCam.fieldOfView;
        targetFOV = 60;
    }

    void AimUpdate()
    {
        if(CurrentWeapon != null)
        {
            CurrentWeapon.transform.localPosition = Vector3.Lerp(PreviousPos, TargetPos, LerpTimer);
            myCam.fieldOfView = Mathf.Lerp(PreviousFOV, targetFOV, LerpTimer);
            LerpTimer = Mathf.Clamp01(LerpTimer + (Time.deltaTime * 10));
        }
    }

    void GrenadeTest()
    {
        if(Input.GetButtonDown("Throw Grenade"))
        {
            // test grenade count
            // throw if allowed.
            // maybe have a grenade throw cooldown.
        }
    }

    void CheckWeapon()
    {
        if (Input.GetButton("Fire"))
        {
            if (CurrentWeapon.Fire(GetTargetBulletLocation))
            {
                PlayerMetricsController.PMC.ShotsFired += CurrentWeapon.ProjectilesPerShot;
                PlayerMetricsController.PMC.BeginCombatTimer();
            }
        }

        if (Input.GetButton("Reload"))
        {
            CurrentWeapon.Reload();
        }

        CurrentWeapon.isAimed = Input.GetButton("Aim");
    }

    void LookForWeapon()
    {
        Ray ray = new Ray(myCam.transform.position, myCam.transform.forward);
        RaycastHit hit = new RaycastHit();
        LayerMask mask = 1<<9;
        EquipText.text = "";
        if (Physics.SphereCast(ray, EquipRadius, out hit, EquipDist, mask))
        {
            if (hit.collider.gameObject.GetComponent<BaseWeapon>().IsEquiped == false)
            {
                EquipText.text = ("Press 'F' to equip: " + hit.collider.name);
                if (Input.GetButton("Interact"))
                {
                    // drop current weapon.
                    CurrentWeapon.OnWeaponUnEquip();
                    CurrentWeapon.transform.SetParent(transform.parent, true);
                    CurrentWeapon.GetComponent<Rigidbody>().AddExplosionForce(5, CurrentWeapon.transform.position - (CurrentWeapon.transform.forward * 0.1f), 0, 0.5f, ForceMode.VelocityChange);
                    CurrentWeapon.WeaponUpdates.Clear();
                    CurrentWeapon.ReloadBeginCallback.Clear();
                    CurrentWeapon.ReloadEndCallback.Clear();

                    // equip targeted weapon.
                    CurrentWeapon = hit.collider.GetComponent<BaseWeapon>();
                    CurrentWeapon.OnWeaponEquip();
                    CurrentWeapon.transform.rotation = Quaternion.identity;
                    CurrentWeapon.transform.position = CurrentWeapon.EquipLocation[0];
                    CurrentWeapon.transform.SetParent(myCam.transform, false);
                    OnWeaponPickup(CurrentWeapon);
                }
            }
        }
    }

    void OnWeaponPickup(BaseWeapon newWeapon)
    {
        CurrentWeapon = newWeapon;
        CurrentWeapon.WeaponUpdates.Add(UserInterfaceController.UIC.OnWeaponUpdate);
        CurrentWeapon.OnWeaponEquip();
        CurrentWeapon.ReloadBeginCallback.Add(UserInterfaceController.UIC.AnimateReload);
    }

    Vector3 GetTargetBulletLocation()
    {
        return myCam.transform.position + (myCam.transform.forward * CurrentWeapon.WeaponRange);
    }

    bool IsPlayerDead()
    {
        return IsDead;
    }

    bool IsPlayerAiming()
    {
        return (CurrentWeapon != null ? CurrentWeapon.isAimed : false);
    }

    void Dying()
    {
        ++PlayerMetricsController.PMC.NumDeaths;
        PlayerMetricsController.PMC.GetNextUnits();
        GameManager.GM.PlayerDied();
    }

    void SetupPlayerStateMachine()
    {
        // Conditions
        BoolCondition IsDeadCond = new BoolCondition(IsPlayerDead);
        NotCondition IsNotDeadCond = new NotCondition(IsDeadCond);

        BoolCondition IsAimingCond = new BoolCondition(IsPlayerAiming);
        NotCondition IsNotAimingCond = new NotCondition(IsAimingCond);

        // Transitions
        Transition Death = new Transition("Death", IsDeadCond, Dying);
        Transition Resurection = new Transition("Resurect", IsNotDeadCond);

        Transition StartAiming = new Transition("Begin Aim", IsAimingCond, BeginAim);
        Transition EndAiming = new Transition("End Aim", IsNotAimingCond, EndAim);

        // States
        State Alive = new State("Alive",
            new List<Transition>() { Death },
            new List<Action>() { BeginAlive },
            new List<Action>() { AliveUpdate },
            new List<Action>() { EndAlive });

        State Dead = new State("Dead",
            new List<Transition>() { Resurection },
            null,
            null,
            null);

        State NotAiming = new State("Not Aiming",
            new List<Transition>() { StartAiming },
            null,
            new List<Action>() { AimUpdate },
            null);

        State Aiming = new State("Aiming",
            new List<Transition>() { EndAiming },
            null,
            new List<Action>() { AimUpdate },
            null);

        // Transition Target States
        Death.SetTargetState(Dead);
        Resurection.SetTargetState(Alive);

        StartAiming.SetTargetState(Aiming);
        EndAiming.SetTargetState(NotAiming);

        // Create Machine
        PSM = new StateMachine(null, Alive, Dead);
        PSM.InitMachine();

        AimStateMachine = new StateMachine(null, Aiming, NotAiming);
        AimStateMachine.InitMachine();
    }

}
