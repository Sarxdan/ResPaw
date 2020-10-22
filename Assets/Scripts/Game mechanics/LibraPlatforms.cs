using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraPlatforms : MonoBehaviour
{
    GameObject[] Platform;
    Collider collider1, collider2;
    [SerializeField] GameObject[] player;
    float Distance;
    // Start is called before the first frame update
    void Start()
    {
        Platform = GameObject.FindGameObjectsWithTag("Platforms");

        player = GameObject.FindGameObjectsWithTag("Player");


    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject players in player)
        {
            CalculateDistance(players.GetComponent<PlayerBase>());
        }

    }
    private void CalculateDistance(PlayerBase Player)
    {
        foreach(GameObject platforms in Platform)
        {
            Distance = Vector3.Distance(Player.transform.position, platforms.transform.position);
        }

        Debug.Log(Distance);
    }
}
