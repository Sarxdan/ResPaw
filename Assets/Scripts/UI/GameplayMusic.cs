using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMusic : MonoBehaviour
{
    public static GameplayMusic Instance { get; private set; }
    [SerializeField]
    int sceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        CheckIfInstance();
    }

    // Update is called once per frame
    void Update()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        DestroyMenuMusic();
    }
    private void DestroyMenuMusic()
    {
        if (sceneIndex == 3 || sceneIndex == 4 || sceneIndex == 5 || sceneIndex == 6)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void CheckIfInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
