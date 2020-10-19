using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{


    public void SpawnPlayer(GameObject animal)
    {

        Vector3 posToSpawn = GetComponent<Transform>().position;

        Instantiate(animal, posToSpawn, GetComponent<Transform>().rotation);
    }

}
