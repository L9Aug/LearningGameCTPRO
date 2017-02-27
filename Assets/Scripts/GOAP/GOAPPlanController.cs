using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanController : MonoBehaviour
{
    public static GOAPPlanController PC;

    Queue<GoapAgent> AgentRequests = new Queue<GoapAgent>();

    bool PlansAllowed = true;

    public int PlansPerFrame = 50;

    void Awake()
    {
        PC = this;
    }

    void Start()
    {
        StartCoroutine(PlanProvider());
    }

    public void RequestPlan(GoapAgent agent)
    {
        if (!AgentRequests.Contains(agent))
        {
            AgentRequests.Enqueue(agent);
        }
    }
	
    IEnumerator PlanProvider()
    {
        int PlansThisFrame = 0;

        while (PlansAllowed)
        {
            if(PlansThisFrame >= PlansPerFrame)
            {
                yield return null;
                PlansThisFrame = 0;
            }

            if (AgentRequests.Count > 0)
            {
                AgentRequests.Dequeue().GetNewPlan();
                ++PlansThisFrame;
            }
            else
            {
                yield return null;
            }
        }
    }

}
