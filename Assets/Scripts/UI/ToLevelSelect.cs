using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class ToLevelSelect : MonoBehaviour
{
    int levelSelect;
    public GameObject pausePanel;
    AudioSource buttons;
    [SerializeField]AudioClip[] pauseButton;
    // Start is called before the first frame update
    
    
    void Start()
    {
        pauseButton = Resources.LoadAll<AudioClip>("Audio/PauseSound");
        buttons = GetComponent<AudioSource>();
        levelSelect = SceneManager.GetActiveScene().buildIndex;
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        ToPause();
    }
    public void ToCharacter()
    {
        buttons.Play();
        SceneManager.LoadScene(1);
        
    }
    public void QuitLevel()
    {
        buttons.Play();
        Application.Quit();
        
    }

    public void LevelSelect()
    {
        buttons.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
        
    }

    public void NextLevel()
    {
        buttons.Play();
        SceneManager.LoadScene(levelSelect + 1);
        
    }

    public void Restart()
    {
        buttons.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(levelSelect);
    }

    public void Resume()
    {
        buttons.Play();
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void ToPause()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }      
    }

    
    




}
