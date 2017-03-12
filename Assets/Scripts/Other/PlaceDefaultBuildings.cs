using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDefaultBuildings : MonoBehaviour
{
    public GameObject BuildingPrefab;
    public List<GameObject> Paths = new List<GameObject>();

    public int X;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject path in Paths)
        {
            for (int x = -X; x <= X; x += 2*X)
            {
                for (int z = -50; z <= 50; z += 10)
                {
                    GameObject tempGO = Instantiate(BuildingPrefab, new Vector3(x, 5, z), Quaternion.identity);
                    tempGO.transform.SetParent(path.transform, false);
                }
            }
        }
    }

}
