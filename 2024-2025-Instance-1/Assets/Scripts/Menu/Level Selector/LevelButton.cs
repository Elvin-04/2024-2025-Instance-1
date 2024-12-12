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

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void Start()
        {
            Assert.IsNotNull(_level, "level is null in LevelButton");
            _btn.interactable = _level.IsUnlocked();
        }
    }
}