using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public DoorController myDoor;

    bool PerformButtonChecks = false;
    Vector3 BasePos;
    bool AnimIsActive = false;

    private void Start()
    {
        BasePos = transform.position;
    }

    private void Update()
    {
        if (PerformButtonChecks)
        {
            Ray ray = new Ray(PlayerController.PC.myCam.transform.position, PlayerController.PC.myCam.transform.forward);
            RaycastHit hit = new RaycastHit();
            LayerMask mask = 1 << 12;
            if (Physics.SphereCast(ray, PlayerController.PC.EquipRadius, out hit, PlayerController.PC.EquipDist, mask, QueryTriggerInteraction.Ignore))
            {
                PlayerController.PC.EquipText.text = "Press 'F' to Open Door.";
            }

            if (Input.GetButtonDown("Interact"))
            {
                if (!GameManager.GM.RoundBegin)
                {
                    PlayerMetricsController.PMC.TimeMissionStarted = Time.realtimeSinceStartup;
                }
                myDoor.OpenDoor();
                if (!AnimIsActive) StartCoroutine(RunAnim());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PerformButtonChecks = true;
    }

    private void OnTriggerExit(Collider other)
    {
        PerformButtonChecks = false;
    }

    IEnumerator RunAnim()
    {
        AnimIsActive = true;
        float timer = 1;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            transform.position = BasePos + (Vector3.forward * (0.2f * Mathf.Sin(timer * Mathf.PI)));
        }
        AnimIsActive = false;
    }
}
