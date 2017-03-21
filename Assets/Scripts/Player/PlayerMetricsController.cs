using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMetricsController : MonoBehaviour
{
    public static PlayerMetricsController PMC;

    public enum NetInputLocations { Accuracy, DamageDealtPerSec, DamageTakenPerSecond, Deaths, ProgressionSpeed }

    public SpawnAI mySpawner;
    public float[] TargetTimePerCheckpoint;

    public float ShotsFired;
    public float ShotsLanded;

    public float TimeInCombat;
    public float DamageDealt;
    public float DamageTaken;

    public int NumDeaths;
    
    public int CheckpointsReached;
    public float TimeMissionStarted;

    bool inCombat;
    float CombatDuration = 3;
    float InCombatTimer;

    private void Start()
    {
        PMC = this;
    }

    float GetAccuracy
    {
        get
        {
            return ShotsFired != 0 ? ShotsLanded / ShotsFired : 1;
        }
    }

    float GetDamageDealtPerSecond
    {
        get
        {
            return TimeInCombat != 0 ? DamageDealt / TimeInCombat : 0;
        }
    }

    float GetDamageTakenPerSecond
    {
        get
        {
            return TimeInCombat != 0 ? DamageTaken / TimeInCombat : 100;
        }
    }

    float GetProgressionSpeed
    {
        get
        {
            int NumChecks = CheckpointsReached >= TargetTimePerCheckpoint.Length ? TargetTimePerCheckpoint.Length - 1 : CheckpointsReached;
            return TargetTimePerCheckpoint[NumChecks] / (Time.realtimeSinceStartup - TimeMissionStarted);
        }
    }

    public void GetNextUnits()
    {
        // Update inputs
        NeuralNet.NeuralNetController myNet = NeuralNet.NeuralNetController.NNC;

        myNet.Inputs[(int)NetInputLocations.Accuracy] = GetAccuracy;
        myNet.Inputs[(int)NetInputLocations.DamageDealtPerSec] = GetDamageDealtPerSecond;
        myNet.Inputs[(int)NetInputLocations.DamageTakenPerSecond] = GetDamageTakenPerSecond;
        myNet.Inputs[(int)NetInputLocations.Deaths] = NumDeaths;
        myNet.Inputs[(int)NetInputLocations.ProgressionSpeed] = GetProgressionSpeed;

        PrintInputs();

        // Begin feedforward
        myNet.RunFeedForward();

    }

    void PrintInputs()
    {
        NeuralNet.NeuralNetController myNet = NeuralNet.NeuralNetController.NNC;

        print(string.Format("Accurracy {0}, DPS {1}, DTPS {2}, Deaths {3}, Prog Speed {4}, Current Time {5}\n",
            myNet.Inputs[(int)NetInputLocations.Accuracy], myNet.Inputs[(int)NetInputLocations.DamageDealtPerSec],
            myNet.Inputs[(int)NetInputLocations.DamageTakenPerSecond], myNet.Inputs[(int)NetInputLocations.Deaths],
            myNet.Inputs[(int)NetInputLocations.ProgressionSpeed], (Time.realtimeSinceStartup - TimeMissionStarted)));
    }

    public void BeginCombatTimer()
    {
        InCombatTimer = CombatDuration;
        if (!inCombat)
        {
            StartCoroutine(CombatTimer());
        }
    }

    IEnumerator CombatTimer()
    {
        inCombat = true;
        while (InCombatTimer > 0)
        {
            yield return null;
            InCombatTimer -= Time.deltaTime;
            TimeInCombat += Time.deltaTime;
        }
        inCombat = false;
    }

}
