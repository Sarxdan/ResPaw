using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private GameObject scoorPreFab;

    [SerializeField]
    private GameObject hightScoors;

    private List<GameObject> gameObjects;


    private ScoreController scoreController;

    bool showForDeaths;
    private int levelShowing;
    void Start()
    {
        scoreController = gameObject.GetComponent<ScoreController>();
        scoreController.ReadListHighScoreTimeForLevel(0);

        scoreController.ReadListScoreTimeForLevel += ReadScoreTimeListForLevel;
        scoreController.ReadListScoreDeathsForLevel += ReadScoreTimeListForLevel;
        gameObjects = new List<GameObject>();

    }

    public void ToHome()
    {

        SceneManager.LoadScene(2);

    }

    public void SetToShowDeaths(bool showDeaths)
    {
        showForDeaths = showDeaths;

        if (showForDeaths)
            scoreController.ReadListHighScoreTimeForLevel(levelShowing);
        else
            scoreController.ReadListHighScoreDeathsForLevel(levelShowing);
    }
    public void ReadListForLevel0()
    {
        levelShowing = 0;
        if (showForDeaths)
            scoreController.ReadListHighScoreTimeForLevel(0);
        else
            scoreController.ReadListHighScoreDeathsForLevel(0);
    }

    public void ReadListForLevel1()
    {
        levelShowing = 1;

        if (showForDeaths)
            scoreController.ReadListHighScoreTimeForLevel(1);
        else
            scoreController.ReadListHighScoreDeathsForLevel(1);
    }
    public void ReadListForLevel2()
    {
        levelShowing = 2;

        if (showForDeaths)
            scoreController.ReadListHighScoreTimeForLevel(2);
        else
            scoreController.ReadListHighScoreDeathsForLevel(2);
    }
    public void ReadListForLevel3()
    {
        levelShowing = 3;

        if (showForDeaths)
            scoreController.ReadListHighScoreTimeForLevel(3);
        else
            scoreController.ReadListHighScoreDeathsForLevel(3);
    }

    public void ReadListForLevel4()
    {
        levelShowing = 4;

        if (showForDeaths)
            scoreController.ReadListHighScoreTimeForLevel(4);
        else
            scoreController.ReadListHighScoreDeathsForLevel(4);
    }
    private void ReadScoreTimeListForLevel(object sender, ScoreCollection scoreCollection)
    {
        RemoveAllScores();

        if (scoreCollection != null)
            if (scoreCollection.scoreList.Count > 0)
            {
                foreach (var score in scoreCollection.scoreList)
                {
                    var text = Instantiate(scoorPreFab);
                    gameObjects.Add(text);


                    TimeSpan result = TimeSpan.FromSeconds(score.time);

                    var textTime = result.ToString("mm':'ss':'ff");
                    var segments = textTime.Split(':');
                    segments[0] = (result.TotalMinutes).ToString("00");

                    var finalText = segments[0] + ":" + segments[1] + ":" + segments[2];
                    string deathsOrTimeText = showForDeaths ? " ** Deaths: " + score.deathsCount : " ** Time: " + finalText;
                    string scoreText = "Name: " + score.name + deathsOrTimeText;
                    text.GetComponent<TMPro.TextMeshProUGUI>().text = scoreText;
                    text.transform.parent = hightScoors.transform;
                    text.transform.localScale = new Vector3(1, 1, 1);
                }
            }

    }

    private void RemoveAllScores()
    {
        foreach (var score in gameObjects)
        {
            Destroy(score);
        }
    }

}
