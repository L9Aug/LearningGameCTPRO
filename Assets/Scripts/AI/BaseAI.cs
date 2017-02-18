using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour
{

    UnityEngine.AI.NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(RandomLocation(500, 0, 500));
        StartCoroutine(Patrol());
        GetComponent<Health>().HealthChangedActions.Add(OnHealthChanged);
    }

    // Update is called once per frame
    void Update()
    {
    }

    Vector3 RandomLocation(float maxX, float maxY, float maxZ)
    {
        return new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), Random.Range(-maxZ, maxZ));
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (agent.remainingDistance < 0.5f)
            {
                agent.SetDestination(RandomLocation(500, 0, 500));
            }
            yield return null;
        }
    }

    void OnHealthChanged(float Amount, float NewHealth, float maxHealth)
    {
        if (NewHealth < 0)
        {
            // This character is dead
            Destroy(gameObject);
        }
    }
}
