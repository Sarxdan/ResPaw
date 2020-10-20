using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

 

     void Update()
    {
         if(Input.GetKeyDown(KeyCode.R)){
             Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            GameManager.Instance.PlayerOneLife = GameManager.Instance.playerLives;
            GameManager.Instance.PlayerTwoLife = GameManager.Instance.playerLives;
         }
     }
 }

