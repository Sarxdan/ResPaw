using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelControll : MonoBehaviour
{
    public static LevelControll instace = null;
    int sceneIndex, levelPassed;

    void Start()
    {
        if (instace == null)
            instace = this;
        else if (instace != null)
            Destroy(gameObject);

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
    }

    public void YouWin()
    {
        if(levelPassed < sceneIndex)
        {
            PlayerPrefs.SetInt("LevelPassed", sceneIndex);
            
        }
        SceneManager.LoadScene(0);
    }
}
