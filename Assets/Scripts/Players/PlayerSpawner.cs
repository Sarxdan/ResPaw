using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public void SpawnPlayer(GameObject animal)
    {

        Vector3 posToSpawn = GetComponent<Transform>().position;

        Instantiate(animal, posToSpawn, GetComponent<Transform>().rotation);
    }

    public void SpawnPlayerFirstTime(Object animalPrefab, int playerNumber)
    {
        Vector3 posToSpawn = GetComponent<Transform>().position;
        GameObject pNewObject = (GameObject)Instantiate(animalPrefab, posToSpawn, GetComponent<Transform>().rotation);
        if (playerNumber == 1)
            pNewObject.AddComponent<PlayerOne>();
        else
            pNewObject.AddComponent<PlayerTwo>();

    }

}
