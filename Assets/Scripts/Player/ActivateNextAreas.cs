using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNextAreas : MonoBehaviour
{
    public List<BoxCollider> SpawnableAreas = new List<BoxCollider>();
    public bool IsFirstTrigger = false;

    bool HasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasBeenTriggered)
        {
            if (IsFirstTrigger) PlayerMetricsController.PMC.TimeMissionStarted = Time.realtimeSinceStartup;
            ++PlayerMetricsController.PMC.CheckpointsReached;
            HasBeenTriggered = true;
            PlayerMetricsController.PMC.GetNextUnits(SpawnableAreas);
        }
    } 

}
