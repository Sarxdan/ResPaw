using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ScoreModel
    {
        public int LevelNumber;

        public int DeathCount;
    }

    [Serializable]
    public class ScoreModelCollection
    {
        public List<ScoreModel> Scores;
    }
}
