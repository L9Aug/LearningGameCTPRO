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
    public SpawnAI mySpawner;
    public Vector3 PlayerSpawnPos;

    public int AIRemaining;
    public int RoundNum = 1;
    public bool RoundBegin = false;

    // Use this for initialization
    void Start()
    {
        GM = this;
        //PlayerMetricsController.PMC.GetNextUnits();
        SpawnFirstWave();
        UpdateRoundNumber();
    }

    void SpawnFirstWave()
    {
        // set specific outputs
        NeuralNet.NeuralNetController myNetCont = NeuralNet.NeuralNetController.NNC;
        myNetCont.Outputs[0] = 0;
        myNetCont.Outputs[1] = 0.5f;
        myNetCont.Outputs[2] = 0.4f;
        myNetCont.Outputs[3] = 0.5f;
        myNetCont.Outputs[4] = 1;
        myNetCont.Outputs[5] = 0;
        myNetCont.Outputs[6] = 0.5f;

        // spawn AI
        mySpawner.spawnAI();
    }

    public void UpdateAICount()
    {
        AIRemainingDisplay.text = AIRemaining.ToString();

        if(AIRemaining <= 0)
        {
            if (!PlayerController.PC.IsDead) PlayerDied();
            ++RoundNum;
            ++PlayerMetricsController.PMC.CheckpointsReached;
            RoundBegin = false;
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
        if (RoundNum <= 5)
        {
            DeathShroud.color = new Color(0, 0, 0, 0);
            if (!PlayerController.PC.IsDead) PlayerMetricsController.PMC.GetNextUnits();

            PlayerController.PC.transform.position = PlayerSpawnPos;
            PlayerController.PC.GetComponent<Health>().SetHealth(PlayerController.PC.GetComponent<Health>().MaxHealth);
            PlayerController.PC.IsDead = false;
        }
        else
        {
            UserInterfaceController.UIC.DisplayGameOver();
        }
    }

}
