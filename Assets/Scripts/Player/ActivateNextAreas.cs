using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNextAreas : MonoBehaviour
{
    public List<BoxCollider> SpawnableAreas = new List<BoxCollider>();

    bool HasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!HasBeenTriggered)
        {
            ++PlayerMetricsController.PMC.CheckpointsReached;
            HasBeenTriggered = true;
            PlayerMetricsController.PMC.GetNextUnits(SpawnableAreas);
        }
    } 

}
