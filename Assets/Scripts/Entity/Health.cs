﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public float health;

    /// <summary>
    /// The amount of health recovered per second.
    /// </summary>
    [Tooltip("The amount of health recovered per second")]
    public float RecoveryAmount;

    /// <summary>
    /// The time in seconds after losing health that recovery can begin again. 
    /// </summary>
    [Tooltip("The time in seconds after losing health that recovery can begin again. ")]
    public float RecoveryDelay;

    public delegate void HealthChanged(float Amount, float NewHealth, float maxHealth);
    public List<HealthChanged> HealthChangedActions = new List<HealthChanged>();

    private bool isRecovering;
    private bool hasBeenHit;

    private float recoveryDelayTimer;

    void Start()
    {
        health = MaxHealth;
        CallHealthChanged(0);
    }

    void CallHealthChanged(float ChangeAmount)
    {
        for(int i = 0; i < HealthChangedActions.Count; ++i)
        {
            if(HealthChangedActions[i] != null)
            {
                HealthChangedActions[i](ChangeAmount, health, MaxHealth);
            }
        }
    }

    public void SetHealth(float Value)
    {
        health = Mathf.Clamp(Value, 0, MaxHealth);
        CallHealthChanged(Value);
    }

    public void AddHealth(float Amount)
    {
        health = Mathf.Clamp(health + Amount, 0, MaxHealth);
        CallHealthChanged(Amount);
    }

    public void Hit(float Damage)
    {
        health -= Damage;
        // Damage goes through as a negative to indicate loss of health.
        CallHealthChanged(-Damage);
        PlayerMetricsController.PMC.BeginCombatTimer();
        BeginRecoveryDelay();
    }

    void BeginRecoveryDelay()
    {
        if (!hasBeenHit)
        {
            StartCoroutine(RecoveryDelayTimer());
        }
        else
        {
            recoveryDelayTimer = 0;
        }
    }

    private IEnumerator HealthRegen()
    {
        if (!isRecovering)
        {
            isRecovering = true;
            while (isRecovering && health < MaxHealth && !hasBeenHit)
            {
                yield return null;
                health = Mathf.Clamp(health + (RecoveryAmount * Time.deltaTime), 0, MaxHealth);
                CallHealthChanged(RecoveryAmount * Time.deltaTime);
            }
            isRecovering = false;
        }
    }

    private IEnumerator RecoveryDelayTimer()
    {
        if (!hasBeenHit) {
            hasBeenHit = true;
            while (hasBeenHit)
            {
                yield return null;
                recoveryDelayTimer += Time.deltaTime;
                if(recoveryDelayTimer >= RecoveryDelay)
                {
                    recoveryDelayTimer = 0;
                    hasBeenHit = false;
                }
            }
            StartCoroutine(HealthRegen());
        }
    }

}
