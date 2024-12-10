using System.Collections.Generic;
using UnityEngine;

namespace Menu.Level_Selector
{
    public class LevelInfo : MonoBehaviour
    {
        public static HashSet<string> completedLevels = new();

        //TODO: A refaire avec des ids
        public string levelName = "LEVEL NAME";
        public string levelScene = "LEVEL SCENE";

        public LevelInfo previousLevel;

        public bool IsUnlocked()
        {
            if (previousLevel == null)
                return true;

            return completedLevels.Contains(previousLevel.levelScene);
        }

        public void MarkComplete()
        {
            completedLevels.Add(levelScene);
        }
    }
}