using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LibraPlatforms : MonoBehaviour
{
    GameObject[] Platform;
    Collider collider1, collider2;
   List<GameObject> gameObjects = new List<GameObject>();
    List<Vector3> orgPos = new List<Vector3>();
    float Distance;
    // Start is called before the first frame update
    void Start()
    {
        Platform = GameObject.FindGameObjectsWithTag("Platforms");

        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

     



        foreach (GameObject plats in Platform)
        {
            orgPos.Add(plats.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

            CalculateDistance();
        

    }
    private void CalculateDistance()
    {
        List<int> platformCount = new List<int>();
        for (int i = 0; i < Platform.Length; i++)
            platformCount.Add(0);

        for (int i = 0; i < Platform.Length; i++)


        {
            GameObject platforms = Platform[i];


            foreach (GameObject entity in gameObjects)
            {
                //PlayerBase tempPlayer = player.GetComponent<PlayerBase>();
                Distance = Vector3.Distance(entity.transform.position, platforms.transform.position);
                if (Distance <= 1)
                {
                    platformCount[i] += 1;
                }
            }
        }
        int mostDensePlatform = platformCount.IndexOf(platformCount.Max());
        Debug.Log(mostDensePlatform);
        /*
        if (moving)
            {
                Vector3 platformPosition = platforms.transform.position;
                platformPosition.y += 0.1f;
                platforms.transform.position = Vector3.MoveTowards(platforms.transform.position, platformPosition, 0.5f * Time.deltaTime);
            }
            else
            {
                platforms.transform.position = Vector3.MoveTowards(platforms.transform.position, orgPos[i], 0.5f * Time.deltaTime);
            }
        }
        */

        
    }
}
