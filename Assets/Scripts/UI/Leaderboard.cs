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
        scoreController.ReadList(1);

        scoreController.ReadScoreListForLevel += ReadScoreListForLevel;
        gameObjects = new List<GameObject>();
    }

    public void ToHome()
    {

        SceneManager.LoadScene(0);

    }


    public void ReadListForLevel1()
    {
        scoreController.ReadList(1);
    }
    public void ReadListForLevel2()
    {
        scoreController.ReadList(2);
    }
    public void ReadListForLevel3()
    {
        scoreController.ReadList(3);
    }

    private void ReadScoreListForLevel(object sender, ScoreCollection scoreCollection)
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
                    string fromTimeString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                    result.Hours,
                    result.Minutes,
                    result.Seconds);
                    string scoreText = "Name: " + score.name + " ** Time: " + fromTimeString;
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
