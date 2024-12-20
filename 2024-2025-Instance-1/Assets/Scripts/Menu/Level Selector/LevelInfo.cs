using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Level_Selector
{
    public class LevelInfo : MonoBehaviour
    {
        private bool _isUnlock = false;
        //TODO: A refaire avec des ids
        public int id;
        public string levelName = "LEVEL NAME";
        public string levelScene = "LEVEL SCENE";

        public LevelInfo previousLevel;
        public LevelInfo nextLevel;


        public bool IsUnlocked => _isUnlock;

        public void CheckUnlocked()
        {
            Save save = new Save();
            SaveObject obj = save.LoadFromJson(id);

            if(obj.score != -1)
                _isUnlock = true;
        }

        public void MarkComplete(int stars)
        {
            Save save = new Save();
            SaveObject obj = new SaveObject(id, stars);
            save.SaveToJson(obj);
        }

        public void Load()
        {
            SceneManager.LoadScene(levelScene);
        }
    }
}