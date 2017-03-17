using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDefaultBuildings : MonoBehaviour
{
    public GameObject BuildingPrefab;
    public List<GameObject> Paths = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        foreach (GameObject path in Paths)
        {
            for(int i = 0; i < 4; ++i)
            {
                switch (i)
                {
                    case 0:
                        for(int x = -50; x < 50; x += 10)
                        {
                            GameObject temp = Instantiate(BuildingPrefab, new Vector3(x, 5, 50), Quaternion.identity);
                            temp.transform.SetParent(path.transform, false);
                        }
                        break;

                    case 1:
                        for (int x = -50; x < 50; x += 10)
                        {
                            GameObject temp = Instantiate(BuildingPrefab, new Vector3(x, 5, -50), Quaternion.identity);
                            temp.transform.SetParent(path.transform, false);
                        }
                        break;

                    case 2:
                        for (int y = -50; y < 50; y += 10)
                        {
                            GameObject temp = Instantiate(BuildingPrefab, new Vector3(50, 5, y), Quaternion.identity);
                            temp.transform.SetParent(path.transform, false);
                        }
                        break;

                    case 3:
                        for (int y = -50; y < 50; y += 10)
                        {
                            GameObject temp = Instantiate(BuildingPrefab, new Vector3(-50, 5, y), Quaternion.identity);
                            temp.transform.SetParent(path.transform, false);
                        }
                        break;                       

                }
            }
        }
    }

}
