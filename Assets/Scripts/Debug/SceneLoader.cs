using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameManager manager;
    private PlayerSpawner playerSpawner1;
    private PlayerSpawner playerSpawner2;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        Object playerOnePrefab = AssetDatabase.LoadAssetAtPath(GetPlayerPreFab(SelectedPlayersSingleton.Instance.SelectedPlayerOne), typeof(GameObject));

        Object playerTwoPrefab = AssetDatabase.LoadAssetAtPath(GetPlayerPreFab(SelectedPlayersSingleton.Instance.SelectedPlayerTwo), typeof(GameObject));


        playerSpawner1 = GameObject.FindGameObjectWithTag("PlayerOneSpawner").GetComponent<PlayerSpawner>();
        playerSpawner2 = GameObject.FindGameObjectWithTag("PlayerTwoSpawner").GetComponent<PlayerSpawner>();

        playerSpawner1.SpawnPlayerFirstTime(playerOnePrefab, 1);
        playerSpawner2.SpawnPlayerFirstTime(playerTwoPrefab, 2);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            manager.PlayerOneLife = manager.playerLives;
            manager.PlayerTwoLife = manager.playerLives;
        }
    }

    private string GetPlayerPreFab(EnumAnimals enumAnimals)
    {
        string preFabeLocation = "Assets/Prefabs/Players/";

        switch (enumAnimals)
        {
            case EnumAnimals.cow:
                preFabeLocation += "Cow.prefab";
                break;
            case EnumAnimals.lion:
                preFabeLocation += "Lion.prefab";
                break;
            case EnumAnimals.bird:
                preFabeLocation += "Bird.prefab";
                break;
            default:
                break;
        }

        return preFabeLocation;
    }
}
