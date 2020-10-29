//Created by Mehmet
//Peer-Reviewed By Daniel
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic Instance { get; private set; }
    [SerializeField] int sceneIndex = 0;
    bool spawned = false;
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
        if(sceneIndex == 0 || sceneIndex == 1 || sceneIndex == 2 )
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
