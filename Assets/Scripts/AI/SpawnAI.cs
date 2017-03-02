using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GOAP;

public class SpawnAI : MonoBehaviour {

    [Range(0, 100)]
    public int NumAiToSpawn = 0;

    public GameObject AIPrefab;

    public List<GoalEnum> Goals = new List<GoalEnum>();

    public enum GoalEnum { Idle, Patrol, Cover, Combat }

	// Use this for initialization
	void Start ()
    {
        // add initial goals.
        // will most likely change after neural net is in place.
        Goals.AddRange(new List<GoalEnum>() { GoalEnum.Idle, GoalEnum.Patrol, GoalEnum.Combat });
        //Goals.Add(new PatrolGoal());

        spawnAI();
	}

    void spawnAI()
    {
        for(int i = 0; i < NumAiToSpawn; ++i)
        {
            GameObject nAI = Instantiate(AIPrefab, GetRandomPosition(), Quaternion.identity, transform);
            GoapAgent nAgent = nAI.GetComponent<GoapAgent>();

            // Add actions before goals
            AddGoals(ref nAgent);
            nAgent.Initialise();
        }
        Debug.Log("Ai Spawned: " + NumAiToSpawn);
    }

    void AddGoals(ref GoapAgent agent)
    {
        //agent.Goals.Add(new typeof(Goals[0])(agent));
        foreach (GoalEnum g in Goals)
        {
            switch (g) 
            {
                case GoalEnum.Idle:
                    agent.Goals.Add(new IdleGoal(agent));
                    break;
                case GoalEnum.Patrol:
                    agent.Goals.Add(new PatrolGoal(agent));
                    break;
                case GoalEnum.Cover:
                    agent.Goals.Add(new CoverGoal(agent));
                    break;
                case GoalEnum.Combat:
                    agent.Goals.Add(new CombatGoal(agent));
                    break;
            }
        }
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
