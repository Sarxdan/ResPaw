using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class ToLevelSelect : MonoBehaviour
{
    int levelSelect;
    public GameObject pausePanel;
    private FinishLine fl;
    private PlayerLives pl;
    //private int prefs; //enable if we want to reset the unlocked levels everytime start is pressed
    AudioSource buttons;
    [SerializeField] AudioClip[] pauseButton;

    [SerializeField]
    private TMP_InputField textMeshPro;

    private bool isUserExist = false;
    // Start is called before the first frame update


    void Start()
    {
        fl = GameObject.FindObjectOfType(typeof(FinishLine)) as FinishLine;
        pl = GameObject.FindObjectOfType(typeof(PlayerLives)) as PlayerLives;
        pauseButton = Resources.LoadAll<AudioClip>("Audio/PauseSound");
        buttons = GetComponent<AudioSource>();
        levelSelect = SceneManager.GetActiveScene().buildIndex;
        pausePanel.SetActive(false);
        isUserExist = string.IsNullOrEmpty(ScoreManager.GetUser().Name) ? false : true;

        if (isUserExist && textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }

    private void Update()
    {

        ToPause();
    }
    public void ToCharacter()
    {
        if (textMeshPro.text != "" || isUserExist)
        {
            if (!isUserExist)
                CreateNewUser();

            buttons.clip = pauseButton[0];
            buttons.Play();
            SceneManager.LoadScene(1);
        }
    }
    public void ToLeaderboard()
    {
        buttons.clip = pauseButton[0];
        buttons.Play();
        SceneManager.LoadScene(7);

    }
    public void QuitLevel()
    {
        buttons.clip = pauseButton[0];
        buttons.Play();
        Application.Quit();

    }

    public void LevelSelect()
    {
        buttons.clip = pauseButton[0];
        buttons.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(2);

    }

    public void NextLevel()
    {
        buttons.clip = pauseButton[0];
        buttons.Play();
        if (levelSelect == 7)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(levelSelect + 1);
        }


    }

    public void Restart()
    {
        buttons.clip = pauseButton[0];
        buttons.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(levelSelect);
    }

    public void Resume()
    {
        buttons.clip = pauseButton[2];
        buttons.Play();
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void ToPause()
    {

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (!fl.winPanel.activeInHierarchy && !pl.lostPanel.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    buttons.clip = pauseButton[1];
                    buttons.Play();
                    pausePanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    private void CreateNewUser()
    {
        ScoreManager.CreateNewUser(textMeshPro.text);
    }







}
