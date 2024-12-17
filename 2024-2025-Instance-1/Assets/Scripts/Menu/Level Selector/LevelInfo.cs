using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Level_Selector
{
    public class LevelInfo : MonoBehaviour
    {
        public static Dictionary<string, int> completedLevels = new();

        //TODO: A refaire avec des ids
        public string levelName = "LEVEL NAME";
        public string levelScene = "LEVEL SCENE";

        public LevelInfo previousLevel;
        public LevelInfo nextLevel;

        public bool IsUnlocked()
        {
            if (previousLevel == null)
                return true;

            return completedLevels.ContainsKey(previousLevel.levelScene);
        }

        public void MarkComplete(int stars)
        {
            completedLevels[levelScene] = stars;
        }

        public void Load()
        {
            SceneManager.LoadScene(levelScene);
        }
    }
}