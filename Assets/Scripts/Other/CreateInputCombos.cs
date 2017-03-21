using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreateInputCombos : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        string InputData = "";
        string OutputData = "";

        for (float Acc = 0.1f; Acc <= 1; Acc += 0.2f)
        {
            for (int DPS = 1; DPS <= 100; DPS += 20)
            {
                for (int DTPS = 0; DTPS <= 100; DTPS += 20)
                {
                    for (int Deaths = 0; Deaths <= 10; ++Deaths)
                    {
                        for (float ProgSpeed = 0.1f; ProgSpeed <= 2; ProgSpeed += 0.2f)
                        {
                            if (!IsInputIllegal(Acc, DPS, DTPS, Deaths, ProgSpeed))
                            {
                                InputData += string.Format("{0},{1},{2},{3},{4}\n", Acc, DPS, DTPS, Deaths, ProgSpeed);
                                OutputData += GetOutputString(Acc, DPS, DTPS, Deaths, ProgSpeed);
                            }
                        }
                    }
                }
            }
        }

        File.WriteAllText(Application.dataPath + "/Resources/NeuralNetFiles/TrainingInputs.txt", InputData);
        File.WriteAllText(Application.dataPath + "/Resources/NeuralNetFiles/TrainingOutputs.txt", OutputData);
    }

    bool IsInputIllegal(float Acc, int DPS, int DTPS, int Deaths, float ProgSpeed)
    {
        if((DPS > 20 && ProgSpeed > 1) || (DTPS == 0 && Deaths > 0))
        {
            return true;
        }
        return false;
    }

    string GetOutputString(float Acc, int DPS, int DTPS, int Deaths, float ProgSpeed)
    {
        string outputs;
        //NumAi = 0, DetectionRadius = 1, CanAim = 2, Accuracy = 3, Goals = 4, Weapon = 5, MaxHealth = 6

        outputs = string.Format("{0},{1},{2},{3},{4},{5},{6}\n",
            Mathf.Clamp01(Mathf.Clamp01(((Acc + (DPS / 100f) + (100f / (DTPS != 0 ? DTPS : 1)) + ProgSpeed) / 4f)) - (Deaths / 20f)), // Num AI = Clamp01(((Acc + (DPS / 100) + (DTPS / 100) + Progspeed) / 4) - (Deaths / 10))
            Mathf.Clamp01((DPS / (float)(DTPS != 0 ? DTPS : 1)) / 2f), // DetectionRadius = Clamp01((DPS / DTPS) / 2)
            Acc, // Can Aim = Acc
            Acc * ProgSpeed, // Accuracy = Acc * prgspeed
            (DPS / (float)(DTPS != 0 ? DTPS : 1)) < 0.1f ? 0.45f : 1, // Goals = DPS / DTPS < 0.1 ? 2 : 4
            (((DPS / 100f) + Acc + ProgSpeed) / 3f), // Weapon = (((DPS / 100) + Acc + ProgSpeed) / 3)
            Mathf.Clamp01((Acc + (DPS / 100f) / 2f))); // Max Health = (Acc + DPS) / 2f

        return outputs;
    }

}
