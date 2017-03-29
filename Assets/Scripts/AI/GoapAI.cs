using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Condition;
using Utility;
using GOAP;

public class GoapAI : MonoBehaviour
{
    public PlayerDetection myDetectionObj;

    public StateMachine myStateMachine;

    public BaseWeapon MyGun;

    GoapAgent myAgent;

    public float DetectionRadius;

    bool isDead = false;
    public bool isAlerted = false;
    public bool CanAimWeapon = true;
    public float LookAccuracy = 0.01f;
    public float ReactionTime = 1;

    public void Initialise()
    {
        myAgent = GetComponent<GoapAgent>();
        GetComponent<Health>().HealthChangedActions.Add(OnHealthChanged);
        SetupStateMachine();
        myDetectionObj.GetComponent<SphereCollider>().radius = DetectionRadius;
    }

    void Update()
    {
        myStateMachine.SMUpdate();
    }

    public void AddWeapon(BaseWeapon weapon)
    {
        // remove any current weapon and add in the new weapon.
        Destroy(MyGun);
        MyGun = (BaseWeapon)Instantiate(weapon);
        MyGun.transform.SetParent(transform, false);
    }

    void OnHealthChanged(float DeltaHealth, float CurrentHealth, float MaxHealth)
    {
        if(DeltaHealth < 0)
        {
            ++PlayerMetricsController.PMC.ShotsLanded;
            PlayerMetricsController.PMC.DamageDealt -= DeltaHealth;
            myDetectionObj.GetComponent<PlayerDetection>().BeginAlerted();
        }

        if(CurrentHealth <= 0)
        {
            isDead = true;
        }
    }

    #region State Machine Functions

    #region Alive State Functions

    void AliveUpdate()
    {
        myAgent.RunPlan();
    }

    void EndAlive()
    {
        myAgent.StopAllActions();
    }

    #endregion

    #region Alerted State Functions

    void BeginAlerted()
    {
        if (myAgent.CurrentWorldState.Find(x => x.Name == "Player Detected") != null)
        {
            myAgent.CurrentWorldState.Find(x => x.Name == "Player Detected").Status = true;
            if (GOAPPlanController.PC != null) GOAPPlanController.PC.RequestPlan(myAgent);
        }
    }

    void AlertedUpdate()
    {

    }

    void EndAlerted()
    {
        if (myAgent.CurrentWorldState.Find(x => x.Name == "Player Detected") != null)
        {
            myAgent.CurrentWorldState.Find(x => x.Name == "Player Detected").Status = false;
        }
    }

    #endregion

    #region Dead State Function

    void DyingFunc()
    {
        --GameManager.GM.AIRemaining;
        GameManager.GM.UpdateAICount();
        Destroy(gameObject);
    }

    #endregion

    #region Conditional Functions

    bool IsDead()
    {
        return isDead;
    }

    bool IsAlerted()
    {
        return isAlerted;
    }

    #endregion

    void SetupStateMachine()
    {
        // Conditions
        BoolCondition IsDeadCond = new BoolCondition(IsDead);
        BoolCondition IsAlertedCond = new BoolCondition(IsAlerted);
        NotCondition IsNotAlertedCond = new NotCondition(IsAlertedCond);
        
        // Transitions
        Transition Dying = new Transition("Dying", IsDeadCond, DyingFunc);
        Transition AlertedTrans = new Transition("Alerted", IsAlertedCond);
        Transition UnAlerted = new Transition("UnAlerted", IsNotAlertedCond);

        // States
        State Alive = new State("Alive",
            new List<Transition>() { Dying, AlertedTrans },
            new List<Action>() { },
            new List<Action>() { AliveUpdate },
            new List<Action>() { EndAlive } );

        State Alerted = new State("Alerted",
            new List<Transition>() { Dying, UnAlerted },
            new List<Action>() { BeginAlerted },
            new List<Action>() { AlertedUpdate, AliveUpdate },
            new List<Action>() { EndAlerted });

        State Dead = new State("Dead",
            null,
            new List<Action>() { },
            new List<Action>() { },
            null);

        // Target States
        Dying.SetTargetState(Dead);
        AlertedTrans.SetTargetState(Alerted);
        UnAlerted.SetTargetState(Alive);

        // init machine
        myStateMachine = new StateMachine(null, Alive, Dead, Alerted);
        myStateMachine.InitMachine();
    }

    #endregion
}
