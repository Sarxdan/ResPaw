using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class ToLevelSelect : MonoBehaviour
{
    int levelselect;
    // Start is called before the first frame update
    void Start()
    {
        levelselect = SceneManager.GetActiveScene().buildIndex + 1;
    }
    public void Tolevel()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitLevel()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(2);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(levelselect);
    }


}
