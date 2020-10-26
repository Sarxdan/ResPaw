using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

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
        if (levelPassed < sceneIndex)
        {
            PlayerPrefs.SetInt("LevelPassed", sceneIndex);           
        }
    }   
}
