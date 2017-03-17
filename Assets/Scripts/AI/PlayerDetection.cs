using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float FOV;
    public GoapAI myAI;

    float AlertedDuration = 3;
    float AlertedTimer;

    bool Reacting = false;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            if(IsPlayerInFov())
            {
                BeginAlerted();
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            if (IsPlayerInFov())
            {
                BeginAlerted();
            }
        }
    }

    bool IsPlayerInFov()
    {
        Vector3 MyForwards = transform.forward;
        Vector3 VecToPlayer = PlayerController.PC.transform.position - transform.position;
        MyForwards.Scale(new Vector3(1, 0, 1));
        VecToPlayer.Scale(new Vector3(1, 0, 1));

        float Angle = Vector3.Angle(MyForwards.normalized, VecToPlayer.normalized);

        if(Angle <= FOV)
        {
            Vector3 myPos = transform.position;
            Vector3 PlayerPos = PlayerController.PC.transform.position;
            // for some reason Unity thought that some y positions were as far as -30 when all were above 0.
            myPos.y = 1;
            PlayerPos.y = 1;

            Ray ray = new Ray(myPos, (PlayerPos - myPos).normalized);
            RaycastHit hit = new RaycastHit();
            int mask = 1 << 9 | 1 << 11;
            mask = ~mask;
            if(Physics.Raycast(ray, out hit, GetComponent<SphereCollider>().radius, mask, QueryTriggerInteraction.Ignore))
            {
                return hit.collider.gameObject.tag == "Player";
            }
        }

        return false;
    }

    public void BeginAlerted()
    {
        if (!Reacting)
        {
            StartCoroutine(AlertedDelay());
        }
        else
        {
            PlayerMetricsController.PMC.BeginCombatTimer();
        }
        
    }

    IEnumerator AlertedDelay()
    {
        Reacting = true;
        float timer = myAI.ReactionTime;
        while(timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        PlayerMetricsController.PMC.BeginCombatTimer();
        AlertedTimer = AlertedDuration;
        if (!myAI.isAlerted)
        {
            StartCoroutine(AlertedTick());
        }
    }

    IEnumerator AlertedTick()
    {
        myAI.isAlerted = true;
        while(AlertedTimer > 0)
        {
            yield return null;
            AlertedTimer -= Time.deltaTime;
        }
        myAI.isAlerted = false;
        Reacting = false;
    }

}
