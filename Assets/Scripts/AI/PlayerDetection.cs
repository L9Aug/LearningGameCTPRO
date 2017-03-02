using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float FOV;
    public GoapAI myAI;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            if(IsPlayerInFov())
            {
                myAI.isAlerted = true;
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            if (IsPlayerInFov())
            {
                myAI.isAlerted = true;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            myAI.isAlerted = false;
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

}
