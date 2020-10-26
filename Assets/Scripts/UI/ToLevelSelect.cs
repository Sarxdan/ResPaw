using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class ToLevelSelect : MonoBehaviour
{
    int levelSelect;
    
    // Start is called before the first frame update
    void Start()
    {
        levelSelect = SceneManager.GetActiveScene().buildIndex;
        
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
        SceneManager.LoadScene(levelSelect + 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(levelSelect);
    }
    
    




}
