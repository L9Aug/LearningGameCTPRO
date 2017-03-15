using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float FOV;
    public GoapAI myAI;

    float AlertedDuration = 3;
    float AlertedTimer;

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
        return Angle <= FOV;
    }

    void BeginAlerted()
    {
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
    }

}
