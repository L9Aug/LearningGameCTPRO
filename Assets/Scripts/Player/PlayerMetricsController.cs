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
            return TimeInCombat != 0 ? DamageTaken / TimeInCombat : 0;
        }
    }

    float GetProgressionSpeed
    {
        get
        {
            int NumChecks = CheckpointsReached >= TargetTimePerCheckpoint.Length ? TargetTimePerCheckpoint.Length - 1 : CheckpointsReached;
            return (Time.realtimeSinceStartup - TimeMissionStarted) / TargetTimePerCheckpoint[NumChecks];
        }
    }

    public void GetNextUnits(List<BoxCollider> SpawnArea)
    {
        // Update inputs
        NeuralNet.NeuralNetController myNet = NeuralNet.NeuralNetController.NNC;

        myNet.Inputs[(int)NetInputLocations.Accuracy] = GetAccuracy;
        myNet.Inputs[(int)NetInputLocations.DamageDealtPerSec] = GetDamageDealtPerSecond;
        myNet.Inputs[(int)NetInputLocations.DamageTakenPerSecond] = GetDamageTakenPerSecond;
        myNet.Inputs[(int)NetInputLocations.Deaths] = NumDeaths;
        myNet.Inputs[(int)NetInputLocations.ProgressionSpeed] = GetProgressionSpeed;

        // inform spawner which areas to 
        mySpawner.SpawnZones.Clear();
        mySpawner.SpawnZones.AddRange(SpawnArea);

        // Begin feedforward
        myNet.RunFeedForward();

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
            CombatDuration += Time.deltaTime;
        }
        inCombat = false;
    }

}
