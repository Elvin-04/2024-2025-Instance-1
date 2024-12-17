using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Menu.Level_Selector
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private LevelInfo _level;

        private Button _btn;
        [SerializeField] private TMP_Text _btnText;

        private void Awake()
        {
            _btn = GetComponent<Button>();

            Assert.IsNotNull(_level, "level is null in LevelButton");
            _btn.interactable = _level.IsUnlocked();

            Assert.IsNotNull(_btnText, "button text is null");
            _btnText.text = _level.levelName;

            if (_level.IsUnlocked() || true)
            {
                if (LevelInfo.completedLevels.TryGetValue(_level.levelScene, out int stars) && stars > 0)
                {
                    _btnText.text += ": ";
                    // ugly
                    for (int i = 0; i < LevelInfo.completedLevels[_level.levelScene]; i++)
                        _btnText.text += "*";
                }
            }
        }
    }
}