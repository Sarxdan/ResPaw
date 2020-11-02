using Assets.Scripts.Models;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//created by Daniel
//peer reviewed by Mehmet

public class ToLevelSelect : MonoBehaviour
{
    int levelSelect;
    public GameObject pausePanel;
    private FinishLine finishLine;
    private PlayerLives playerLives;
    public int prefs; //enable if we want to reset the unlocked levels everytime start is pressed
    AudioSource buttons;
    [SerializeField] AudioClip[] pauseButton;

    [SerializeField]
    private TMP_InputField playerNameInput;

    private ScoreModel scoreModel;

    // Start is called before the first frame update
    private ScoreController scoreController;

    void Start()
    {
        scoreModel = null;
        try
        {
            playerNameInput.characterLimit = 25;
            scoreController = gameObject.GetComponent<ScoreController>();
        }
        catch (Exception)
        {


        }

        finishLine = GameObject.FindObjectOfType(typeof(FinishLine)) as FinishLine;
        playerLives = GameObject.FindObjectOfType(typeof(PlayerLives)) as PlayerLives;
        pauseButton = Resources.LoadAll<AudioClip>("Audio/PauseSound");
        buttons = GetComponent<AudioSource>();
        levelSelect = SceneManager.GetActiveScene().buildIndex;
        pausePanel.SetActive(false);
        prefs = PlayerPrefs.GetInt("LevelPassed");

    }

    private void Update()
    {

        ToPause();
    }
    public void ToCharacter()
    {
        if(prefs < 1)
        {
            PlayerPrefs.DeleteAll();
        }
        
        buttons.clip = pauseButton[0];
        buttons.Play();
        SceneManager.LoadScene(1);

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
        SaveScoreIfWin();
        buttons.clip = pauseButton[0];
        buttons.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(2);

    }

    public void NextLevel()
    {
        buttons.clip = pauseButton[0];
        buttons.Play();

        SaveScoreIfWin();

        if (levelSelect == 7)
        {
            this.Restart();

            SceneManager.LoadScene(2);
        }
        else
        {
            this.Restart();

            SceneManager.LoadScene(levelSelect + 1);
        }



    }

    private void SaveScoreIfWin()
    {
        if (scoreModel != null)
            SaveScoreInServer(scoreModel);
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
            if (!finishLine.winPanel.activeInHierarchy && !playerLives.lostPanel.activeInHierarchy)
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

    public void SetScore(ScoreModel scoreModel)
    {
        this.scoreModel = scoreModel;
    }

    private void SaveScoreInServer(ScoreModel scoreModel)
    {

        Score score = new Score()
        {
            name = string.IsNullOrEmpty(playerNameInput.text) ? "WithoutName" : playerNameInput.text,
            userId = Guid.NewGuid().ToString(),
            dateAdded = DateTime.Now.ToString(),
            deathsCount = scoreModel.DeathCount,
            levelNumber = scoreModel.LevelNumber,
            time = scoreModel.Time
        };

        scoreController.SaveScroe(score);


    }





}
