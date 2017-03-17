using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public DoorController myDoor;

    private void OnTriggerEnter(Collider other)
    {
        myDoor.CloseDoor();
    }

}
