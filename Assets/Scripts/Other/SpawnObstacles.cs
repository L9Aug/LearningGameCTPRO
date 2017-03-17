using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{

    public GameObject ObstaclePrefab;

    void Start()
    {
        int NumObjs = Random.Range(25, 50);

        for (int i = 0; i < NumObjs; ++i)
        {
            GameObject TempObj = Instantiate(ObstaclePrefab);
            TempObj.transform.localScale = new Vector3(Random.Range(0.5f, 5f), Random.Range(1f, 5f), Random.Range(0.5f, 5f));
            TempObj.transform.position = new Vector3(Random.Range(-50f, 50f), TempObj.transform.localScale.y / 2f, Random.Range(-50f, 50f));
            TempObj.transform.Rotate(Vector3.up, Random.Range(0, 180));
            TempObj.transform.SetParent(transform);
        }
    }
}
