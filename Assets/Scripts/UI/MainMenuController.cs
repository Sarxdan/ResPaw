using Assets.Scripts.Models;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//created by Daniel
//peer reviewed by Mehmet

public class MainMenuController : MonoBehaviour
{
    public Button button2, button3, button4, button5;

    [SerializeField]
    private TextMeshProUGUI[] DeathsCountsList;

    [SerializeField]
    private TextMeshProUGUI[] TimeCountsList;

    public int levelpassed;

    private ScoreController scoreController;
    void Start()
    {
        scoreController = gameObject.GetComponent<ScoreController>();

        scoreController.ReadScoreDeathForLevel += SetTextOnDeathsCount;
        scoreController.ReadScoreTimeForLevel += SetTextOnTimesCount;


        ReadAllLevelTopDeathsFromServer();
        ReadAllLevelTopTimeFromServer();
        levelpassed = PlayerPrefs.GetInt("LevelPassed");
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;
        button5.interactable = false;

        switch (levelpassed)
        {
            case 3:
                button2.interactable = true;
                break;
            case 4:
                button2.interactable = true;
                button3.interactable = true;
                break;
            case 5:
                button2.interactable = true;
                button3.interactable = true;
                button4.interactable = true;
                break;
            case 6:
                button2.interactable = true;
                button3.interactable = true;
                button4.interactable = true;
                button5.interactable = true;
                break;
            case 7:
                button2.interactable = true;
                button3.interactable = true;
                button4.interactable = true;
                button5.interactable = true;
                break;

        }




    }

    public void LevelToLoad(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);//change 0 to whatever is mainmenu
    }
    public void reset()
    {
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;
        button5.interactable = false;
        PlayerPrefs.DeleteAll();
    }



    private void ReadAllLevelTopDeathsFromServer()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreController.ReadHighScoreDeathsForLevel(i);
        }
    }
    private void ReadAllLevelTopTimeFromServer()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreController.ReadHighScoreTimeForLevel(i);
        }
    }

    private void SetTextOnDeathsCount(object sender, Score score)
    {
        if (score != null)
            DeathsCountsList[score.levelNumber].text = score.deathsCount.ToString();
    }

    private void SetTextOnTimesCount(object sender, Score score)
    {
        if (score != null)
        {
            TimeSpan result = TimeSpan.FromSeconds(score.time);

            var textTime = result.ToString("mm':'ss':'ff");
            var segments = textTime.Split(':');
            segments[0] = (result.TotalMinutes).ToString("00");

            var finalText = segments[0] + ":" + segments[1] + ":" + segments[2];

            TimeCountsList[score.levelNumber].text = finalText;
        }
    }

}
