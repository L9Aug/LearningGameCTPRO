using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNextAreas : MonoBehaviour
{

    public int CheckpointNum;
    public List<Collider> SpawnableAreas = new List<Collider>();

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
