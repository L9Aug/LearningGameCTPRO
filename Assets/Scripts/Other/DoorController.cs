using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    Animator Anim;
    int OpenDoorTriggerHash;
    int CloseDoorTriggerHash;

    // Use this for initialization
    void Start()
    {
        Anim = GetComponent<Animator>();
        OpenDoorTriggerHash = Animator.StringToHash("OpenDoor");
        CloseDoorTriggerHash = Animator.StringToHash("CloseDoor");
    }

    public void OpenDoor()
    {
        Anim.SetTrigger(OpenDoorTriggerHash);
    }

    public void CloseDoor()
    {
        Anim.SetTrigger(CloseDoorTriggerHash);
    }

}
