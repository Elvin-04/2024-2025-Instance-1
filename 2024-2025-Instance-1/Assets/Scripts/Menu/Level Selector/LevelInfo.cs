using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelInfo : MonoBehaviour
{
    public static HashSet<string> completedLevels = new HashSet<string>();

    public string levelName  = "LEVEL NAME";
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