using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class ToLevelSelect : MonoBehaviour
{
    int levelSelect;
    public GameObject pausePanel;
    
    // Start is called before the first frame update
    
    
    void Start()
    {
        levelSelect = SceneManager.GetActiveScene().buildIndex;
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        ToPause();
    }
    public void QuitLevel()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(levelSelect + 1);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelSelect);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void ToPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    
    




}
