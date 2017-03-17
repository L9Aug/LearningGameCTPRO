using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FSM;
using Condition;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public Image DeathShroud;
    public Text AIRemainingDisplay;
    public Text RoundDisplay;
    public Vector3 PlayerSpawnPos;

    public int AIRemaining;
    public int RoundNum = 1;

    // Use this for initialization
    void Start()
    {
        GM = this;
        PlayerMetricsController.PMC.GetNextUnits();
        UpdateRoundNumber();
    }

    public void UpdateAICount()
    {
        AIRemainingDisplay.text = AIRemaining.ToString();

        if(AIRemaining <= 0)
        {
            if (!PlayerController.PC.IsDead) PlayerDied();
            ++RoundNum;
            UpdateRoundNumber();
        }
    }

    public void UpdateRoundNumber()
    {
        RoundDisplay.text = RoundNum.ToString();
    }

    public void PlayerDied()
    {
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        float timer = 2;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            DeathShroud.color = new Color(DeathShroud.color.r, DeathShroud.color.g, DeathShroud.color.b, Mathf.Lerp(1, 0, timer / 2f));
        }
        DeathShroud.color = new Color(0, 0, 0, 0);
        if (!PlayerController.PC.IsDead) PlayerMetricsController.PMC.GetNextUnits();

        PlayerController.PC.transform.position = PlayerSpawnPos;
        PlayerController.PC.GetComponent<Health>().SetHealth(PlayerController.PC.GetComponent<Health>().MaxHealth);
        PlayerController.PC.IsDead = false;
    }

}
