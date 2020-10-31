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
    void Start()
    {
        scoreController = gameObject.GetComponent<ScoreController>();
        scoreController.ReadListHighScoreTimeForLevel(1);

        scoreController.ReadListScoreTimeForLevel += ReadScoreTimeListForLevel;
        gameObjects = new List<GameObject>();
    }

    public void ToHome()
    {

        SceneManager.LoadScene(0);

    }


    public void ReadListForLevel1()
    {
        scoreController.ReadListHighScoreTimeForLevel(1);
    }
    public void ReadListForLevel2()
    {
        scoreController.ReadListHighScoreTimeForLevel(2);
    }
    public void ReadListForLevel3()
    {
        scoreController.ReadListHighScoreTimeForLevel(3);
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
                    string scoreText = "Name: " + score.name + " ** Time: " + finalText;
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
