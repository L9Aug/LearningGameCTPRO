using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GOAP;

public class SpawnAI : MonoBehaviour {

    [Range(0, 100)]
    public int NumAiToSpawn = 0;

    public GameObject AIPrefab;

    public List<GoapGoal> Goals = new List<GoapGoal>();

	// Use this for initialization
	void Start ()
    {
        // add initial goals.
        // will most likely change after neural net is in place.
        Goals.Add(new IdleGoal());

        spawnAI();
	}

    void spawnAI()
    {
        for(int i = 0; i < NumAiToSpawn; ++i)
        {
            GameObject nAI = Instantiate(AIPrefab, GetRandomPosition(), Quaternion.identity, transform);
            nAI.GetComponent<GoapAgent>().Goals.AddRange(Goals);
        }
        Debug.Log("Ai Spawned: " + NumAiToSpawn);
    }

    Vector3 GetRandomPosition()
    {
        Vector3 returnVec = new Vector3(Random.Range(-500, 500), 0.0f, Random.Range(-500, 500));

        UnityEngine.AI.NavMeshHit hit;

        if(UnityEngine.AI.NavMesh.SamplePosition(returnVec, out hit, 100, UnityEngine.AI.NavMesh.AllAreas))
        {
            returnVec = hit.position;
        }

        return returnVec;
    }
}
