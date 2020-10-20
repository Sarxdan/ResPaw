using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

 

     void Update()
    {
         if(Input.GetKeyDown(KeyCode.R)){
             Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            manager.PlayerOneLife = manager.playerLives;
            manager.PlayerTwoLife = manager.playerLives;
         }
     }
 }

