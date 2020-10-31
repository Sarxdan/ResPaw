using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameManager manager;
    private PlayerSpawner playerSpawner1;
    private PlayerSpawner playerSpawner2;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        try
        {
            Object[] allAnimalsPrefab = Resources.LoadAll<GameObject>("Players");
            Object playerOnePrefab = new Object();
            Object playerTwoPrefab = new Object();
            foreach (var prefab in allAnimalsPrefab)
            {
                if (prefab.name == GetPlayerPreFab(SelectedPlayersSingleton.Instance.SelectedPlayerOne))
                {
                    playerOnePrefab = prefab;
                }
                if (prefab.name == GetPlayerPreFab(SelectedPlayersSingleton.Instance.SelectedPlayerTwo))
                {
                    playerTwoPrefab = prefab;
                }
            }





            playerSpawner1 = GameObject.FindGameObjectWithTag("PlayerOneSpawner").GetComponent<PlayerSpawner>();
            playerSpawner2 = GameObject.FindGameObjectWithTag("PlayerTwoSpawner").GetComponent<PlayerSpawner>();

            playerSpawner1.SpawnPlayerFirstTime(playerOnePrefab, 1);
            playerSpawner2.SpawnPlayerFirstTime(playerTwoPrefab, 2);
        }
        catch (System.Exception)
        {
            Object[] allAnimalsPrefab = Resources.LoadAll<GameObject>("Players");
            Object playerOnePrefab = new Object();
            Object playerTwoPrefab = new Object();
            foreach (var prefab in allAnimalsPrefab)
            {
                if (prefab.name == GetPlayerPreFab(EnumAnimals.cow))
                {
                    playerOnePrefab = prefab;
                }
                if (prefab.name == GetPlayerPreFab(EnumAnimals.lion))
                {
                    playerTwoPrefab = prefab;
                }
            }





            playerSpawner1 = GameObject.FindGameObjectWithTag("PlayerOneSpawner").GetComponent<PlayerSpawner>();
            playerSpawner2 = GameObject.FindGameObjectWithTag("PlayerTwoSpawner").GetComponent<PlayerSpawner>();

            playerSpawner1.SpawnPlayerFirstTime(playerOnePrefab, 1);
            playerSpawner2.SpawnPlayerFirstTime(playerTwoPrefab, 2);

        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !IsAnyMenuActive())
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            manager.PlayerOneLife = manager.playerLives;
            manager.PlayerTwoLife = manager.playerLives;
        }
    }
    private bool IsAnyMenuActive()
    {
        return pausePanel.activeInHierarchy || losePanel.activeInHierarchy || winPanel.activeInHierarchy;
    }
    private string GetPlayerPreFab(EnumAnimals enumAnimals)
    {
        string preFabeLocation = "";

        switch (enumAnimals)
        {
            case EnumAnimals.cow:
                preFabeLocation += "Cow";
                break;
            case EnumAnimals.lion:
                preFabeLocation += "Lion";
                break;
            case EnumAnimals.cat:
                preFabeLocation += "Cat";
                break;
            case EnumAnimals.dog:
                preFabeLocation += "Dog";
                break;
            case EnumAnimals.panda:
                preFabeLocation += "Panda";
                break;
            case EnumAnimals.turtle:
                preFabeLocation += "Turtle";
                break;
            default:
                break;
        }

        return preFabeLocation;
    }
}
