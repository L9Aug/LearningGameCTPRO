using UnityEngine;
using System.Collections;

public class SpawnAI : MonoBehaviour {

    [Range(0, 100)]
    public int NumAiToSpawn = 0;

    public GameObject AIPrefab;

	// Use this for initialization
	void Start ()
    {
        spawnAI();
	}
	
	// Update is called once per frame
	void Update ()
    {	    

	}

    void spawnAI()
    {
        for(int i = 0; i < NumAiToSpawn; ++i)
        {
            Instantiate(AIPrefab, GetRandomPosition(), Quaternion.identity, transform);
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
