using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ScoreModel
    {
        public int LevelNumber;

        public int DeathCount;
        public int Time;
    }

    [Serializable]
    public class ScoreModelCollection
    {
        public List<ScoreModel> Scores;
    }
}
