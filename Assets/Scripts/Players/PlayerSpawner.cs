using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField]
    private Transform spawnPosition;


    public void SpawnPlayer(GameObject animal)
    {

        Vector3 posToSpawn = spawnPosition.position;

        Instantiate(animal, posToSpawn, spawnPosition.rotation);
    }

}
