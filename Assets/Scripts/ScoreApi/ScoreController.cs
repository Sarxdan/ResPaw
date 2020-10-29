using Assets.Scripts.Models;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreController : MonoBehaviour
{
    public event EventHandler<Score> ReadScoreForLevel;
    public event EventHandler<ScoreCollection> ReadScoreListForLevel;
    public void SaveScroe(Score score)
    {
        StartCoroutine(PostNewScore(score));
    }

    public void ReadHighScoreForLevel(int levelNumber)
    {
        StartCoroutine(GetHighScoreForLevel(levelNumber));

    }


    public void ReadList(int LevelNumber)
    {
        StartCoroutine(GetListHighScoreForLevel(LevelNumber));
    }



    IEnumerator PostNewScore(Score score)
    {
        WWWForm scoreForm = new WWWForm();

        scoreForm.AddField("UserId", score.userId);
        scoreForm.AddField("Name", score.name);
        scoreForm.AddField("LevelNumber", score.levelNumber);
        scoreForm.AddField("Time", score.time);
        scoreForm.AddField("DeathsCount", score.deathsCount);
        scoreForm.AddField("DateAdded", "31/03/2014");
        scoreForm.AddField("Id", 0);



        using (UnityWebRequest www = UnityWebRequest.Post("http://www.menkeesi.com:5007/api/score/SaveScore", scoreForm))
        {

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    IEnumerator GetHighScoreForLevel(int levelNumber)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://www.menkeesi.com:5007/api/score/GetHieghScore?levelNumber=" + levelNumber))
        {

            yield return webRequest.SendWebRequest();

            var result = webRequest.downloadHandler.text;


            var highScoreForLevel = JsonUtility.FromJson<Score>(result);

            ReadScoreForLevel?.Invoke(this, highScoreForLevel);

        }
    }
    IEnumerator GetListHighScoreForLevel(int levelNumber)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://www.menkeesi.com:5007/api/score/GetListHieghScoreForLevel?levelNumber=" + levelNumber))
        {

            yield return webRequest.SendWebRequest();

            var result = webRequest.downloadHandler.text;


            var highScoreListForLevel = JsonUtility.FromJson<ScoreCollection>(result);
            ReadScoreListForLevel?.Invoke(this, highScoreListForLevel);


        }
    }
}
