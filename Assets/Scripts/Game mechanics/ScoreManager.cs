using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager
{

    public static void CreateNewUser(string name)
    {
        User user = new User()
        {
            Name = name,
            UserId = Guid.NewGuid().ToString()
        };

        if (String.IsNullOrEmpty(GetUser().Name))
        {
            string data = JsonUtility.ToJson(user, true);
            File.WriteAllText(Application.persistentDataPath + "/UserInfo.json", data);
        }
    }

    public static User GetUser()
    {
        try
        {
            using (StreamReader r = new StreamReader(Application.persistentDataPath + "/UserInfo.json"))
            {
                string json = r.ReadToEnd();
                User user = JsonUtility.FromJson<User>(json);

                return user;
            }
        }
        catch (System.Exception)
        {

            return new User();
        }
    }

    public static void SavewNewScore(ScoreModel score)
    {
        var AllScores = GetScores();
        if (IsNewHighScore(ref AllScores, score))
        {

            AllScores.Scores.Add(score);
            string data = JsonUtility.ToJson(AllScores, true);

            File.WriteAllText(Application.persistentDataPath + "/SavedScores.json", data);
        }


    }

    private static bool IsNewHighScore(ref ScoreModelCollection allScores, ScoreModel score)
    {
        var oldScore = allScores.Scores.Find(storedscore => storedscore.LevelNumber == score.LevelNumber);
        if (oldScore != null)
            if (oldScore.DeathCount > score.DeathCount)
            {
                allScores.Scores.Remove(oldScore);
                return true;
            }
            else
            {
                return false;
            }
        return true;
    }

    public static ScoreModelCollection GetScores()
    {

        try
        {
            using (StreamReader r = new StreamReader(Application.persistentDataPath + "/SavedScores.json"))
            {
                string json = r.ReadToEnd();
                ScoreModelCollection scores = JsonUtility.FromJson<ScoreModelCollection>(json);

                return scores;
            }
        }
        catch (System.Exception)
        {

            return new ScoreModelCollection()
            {
                Scores = new List<ScoreModel>()
            };
        }
    }

}
