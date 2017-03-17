using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GOAP;
using NeuralNet;

public class SpawnAI : MonoBehaviour
{

    [Range(0, 100)]
    public int NumAiToSpawn = 0;
    public GameObject AIPrefab;
    public bool UseNetwork;

    public int MinAIToSpawn;
    public int MaxAIToSpawn;
    public int MinDetectionRadius;
    public int MaxDetectionRadius;
    public float MinAccuracy;
    public float MaxAccuracy;
    public List<BaseWeapon> Weapons = new List<BaseWeapon>();
    public List<GoalEnum> Goals = new List<GoalEnum>();

    public enum GoalEnum { Idle, Patrol, Combat, Cover }

    enum NetworkOutputNames { NumAi = 0, DetectionRadius = 1, CanAim = 2, Accuracy = 3, Goals = 4, Weapon = 5, MaxHealth  = 6 }

    public List<BoxCollider> SpawnZones = new List<BoxCollider>();

	// Use this for initialization
	void Start ()
    {
        if (!NeuralNetController.NNC.FeedforwardCallBacks.Contains(spawnAI))
        {
            NeuralNetController.NNC.FeedforwardCallBacks.Add(spawnAI);
        }
    }

    void spawnAI()
    {
        if (UseNetwork)
        {
            NeuralNetController myNet = NeuralNetController.NNC;
            int NetworkAiToSpawn = (int)Mathf.Lerp(MinAIToSpawn, MaxAIToSpawn, myNet.Outputs[(int)NetworkOutputNames.NumAi]);
            int DetectionRadius = (int)Mathf.Lerp(MinDetectionRadius, MaxDetectionRadius, myNet.Outputs[(int)NetworkOutputNames.DetectionRadius]);
            bool CanAim = myNet.Outputs[(int)NetworkOutputNames.CanAim] > 0.5f;
            float Accuracy = Mathf.Lerp(MinAccuracy, MaxAccuracy, myNet.Outputs[(int)NetworkOutputNames.Accuracy]);
            int numGoals = (int)Mathf.Lerp(1, 4, myNet.Outputs[(int)NetworkOutputNames.Goals]);
            BaseWeapon WeaponForAI = Weapons[Mathf.RoundToInt(Mathf.Lerp(0, Weapons.Count - 1, myNet.Outputs[(int)NetworkOutputNames.Weapon]))];
            float MaxHealth = Mathf.Lerp(50, 200, myNet.Outputs[(int)NetworkOutputNames.MaxHealth]);

            for (int i = 0; i < NetworkAiToSpawn; ++i)
            {
                // Get Position within Next Area.
                GameObject nAI = Instantiate(AIPrefab, GetRandomPosition(), Quaternion.identity, transform);
                GoapAgent tempAgent = nAI.GetComponent<GoapAgent>();
                AddGoals(ref tempAgent, numGoals);

                GoapAI myAIComp = nAI.GetComponent<GoapAI>();
                myAIComp.DetectionRadius = DetectionRadius;
                myAIComp.CanAimWeapon = CanAim;
                myAIComp.LookAccuracy = Accuracy;
                myAIComp.AddWeapon(WeaponForAI);

                Health AIHealth = nAI.GetComponent<Health>();
                AIHealth.MaxHealth = MaxHealth;

                tempAgent.Initialise();
                myAIComp.Initialise();
            }          

            GameManager.GM.AIRemaining = NetworkAiToSpawn;
            GameManager.GM.UpdateAICount();
        }
    }

    void AddGoals(ref GoapAgent agent, int Goals)
    {
        for(int i = 0; i < Goals; ++i)
        {
            AddGoal(ref agent, (GoalEnum)i);
        }
    }

    void AddGoal(ref GoapAgent agent, GoalEnum goal)
    {
        switch (goal)
        {
            case GoalEnum.Idle:
                agent.Goals.Add(new IdleGoal(agent));
                break;
            case GoalEnum.Patrol:
                agent.Goals.Add(new PatrolGoal(agent));
                break;
            case GoalEnum.Combat:
                agent.Goals.Add(new CombatGoal(agent));
                break;
            case GoalEnum.Cover:
                agent.Goals.Add(new CoverGoal(agent));
                break;
        }
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
                case GoalEnum.Combat:
                    agent.Goals.Add(new CombatGoal(agent));
                    break;
                case GoalEnum.Cover:
                    agent.Goals.Add(new CoverGoal(agent));
                    break;
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 returnVec = new Vector3(Random.Range(-45, 45), 0.0f, Random.Range(-45, 45));

        UnityEngine.AI.NavMeshHit hit;

        if(UnityEngine.AI.NavMesh.SamplePosition(returnVec, out hit, 100, UnityEngine.AI.NavMesh.AllAreas))
        {
            returnVec = hit.position;
        }

        return returnVec;
    }
}
