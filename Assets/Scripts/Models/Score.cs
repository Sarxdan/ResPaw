using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Score
    {
        public int id;

        public string userId;
        public string name;

        public int levelNumber;
        public float time;

        public int deathsCount;

        public string dateAdded;
    }

    [Serializable]
    public class ScoreCollection
    {
        public List<Score> scoreList;
    }
}
