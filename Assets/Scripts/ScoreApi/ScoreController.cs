using Assets.Scripts.Models;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreController : MonoBehaviour
{
    public event EventHandler<Score> ReadScoreTimeForLevel;
    public event EventHandler<Score> ReadScoreDeathForLevel;
    public event EventHandler<ScoreCollection> ReadListScoreTimeForLevel;
    public event EventHandler<ScoreCollection> ReadListScoreDeathsForLevel;


    public void SaveScroe(Score score)
    {
        StartCoroutine(PostNewScore(score));
    }

    public void ReadHighScoreTimeForLevel(int levelNumber)
    {
        StartCoroutine(GetHighScoreTimeForLevel(levelNumber));

    }
    public void ReadHighScoreDeathsForLevel(int levelNumber)
    {
        StartCoroutine(GetHighScoreDeathsForLevel(levelNumber));

    }


    public void ReadListHighScoreTimeForLevel(int LevelNumber)
    {
        StartCoroutine(GetListHighScoreTimeForLevel(LevelNumber));
    }

    public void ReadListHighScoreDeathsForLevel(int LevelNumber)
    {
        StartCoroutine(GetListHighScoreDeathsForLevel(LevelNumber));
    }



    IEnumerator PostNewScore(Score score)
    {
        WWWForm scoreForm = new WWWForm();

        scoreForm.AddField("UserId", score.userId);
        scoreForm.AddField("Name", score.name);
        scoreForm.AddField("LevelNumber", score.levelNumber);
        scoreForm.AddField("Time", score.time.ToString());
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


    IEnumerator GetHighScoreTimeForLevel(int levelNumber)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://www.menkeesi.com:5007/api/score/GetHighScoreTimeForLevel?levelNumber=" + levelNumber))
        {

            yield return webRequest.SendWebRequest();

            var result = webRequest.downloadHandler.text;


            var highScoreForLevel = JsonUtility.FromJson<Score>(result);

            ReadScoreTimeForLevel?.Invoke(this, highScoreForLevel);

        }
    }
    IEnumerator GetHighScoreDeathsForLevel(int levelNumber)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://www.menkeesi.com:5007/api/score/GetHighScoreDeathsForLevel?levelNumber=" + levelNumber))
        {

            yield return webRequest.SendWebRequest();

            var result = webRequest.downloadHandler.text;


            var highScoreForLevel = JsonUtility.FromJson<Score>(result);

            ReadScoreDeathForLevel?.Invoke(this, highScoreForLevel);

        }
    }
    IEnumerator GetListHighScoreTimeForLevel(int levelNumber)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://www.menkeesi.com:5007/api/score/GetListHighScoreTimeForLevel?levelNumber=" + levelNumber))
        {

            yield return webRequest.SendWebRequest();

            var result = webRequest.downloadHandler.text;


            var highScoreListForLevel = JsonUtility.FromJson<ScoreCollection>(result);
            ReadListScoreTimeForLevel?.Invoke(this, highScoreListForLevel);


        }
    }
    IEnumerator GetListHighScoreDeathsForLevel(int levelNumber)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://www.menkeesi.com:5007/api/score/GetListHighScoreDeathsForLevel?levelNumber=" + levelNumber))
        {

            yield return webRequest.SendWebRequest();

            var result = webRequest.downloadHandler.text;


            var highScoreListForLevel = JsonUtility.FromJson<ScoreCollection>(result);
            ReadListScoreDeathsForLevel?.Invoke(this, highScoreListForLevel);


        }
    }
}
