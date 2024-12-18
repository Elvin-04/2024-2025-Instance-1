using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu.Level_Selector
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private GameObject _boxLevel;
        [SerializeField] private List<LevelInfo> _levels = new();
        private List<Index> _indexes = new();

        [Header("Index")]
        [SerializeField] private GameObject _indexPrefab;
        [SerializeField] private Transform _indexTransform;
        [SerializeField] private int _distanceBetweenIndex = 20;

        [Header("Stars")]
        [SerializeField] private RawImage[] _starsImage = new RawImage[3];
        [SerializeField] private Texture _star;
        [SerializeField] private Texture _noStar;
        private Save _save;

        private int _currentLevel;
        public LevelInfo currentLevel => _levels[_currentLevel];

        private int _currentLevelStars = 0;

        private void Awake()
        {
            _save = new Save();
            Debug.Log(_save);

            SetLevels();
        }

        private void Start()
        {
            _save.SaveToJson(new SaveObject(0, 0));
        }
        private void SetStar(int stars)
        {
            for(int i = 0; i< _starsImage.Length; i++)
            {
                if(i < stars)
                    _starsImage[i].texture = _star;
                else
                    _starsImage[i].texture = _noStar;
            }
        }
        private void SetLevels()
        {
            foreach (var l in _boxLevel.GetComponentsInChildren<LevelInfo>()) 
            {
                _levels.Add(l);
                l.gameObject.SetActive(false);
            }

            for (int i = 0; i < _levels.Count; i++)
            {
                _levels[i].nextLevel = (i + 1 < _levels.Count) ? _levels[i + 1] : _levels[0];
                _levels[i].previousLevel = (i - 1 >= 0) ? _levels[i - 1] : _levels[_levels.Count - 1];
            }
            _currentLevel = 0;
            _levels[0].gameObject.SetActive(true);

            float startOffset = -(_levels.Count - 1) * _distanceBetweenIndex / 2;

            for (int i = 0; i < _levels.Count; i++)
            {
                _levels[i].id = i;
                _levels[i].CheckUnlocked();
                Vector3 position = _indexTransform.position + new Vector3(startOffset + i * _distanceBetweenIndex, 0, 0);
                GameObject index = Instantiate(_indexPrefab, position, Quaternion.identity, _indexTransform);
                _indexes.Add(index.GetComponent<Index>());
                _indexes[i].ChangeColor(GetState(_levels[i]));
            }
            SetStar(_save.LoadFromJson(currentLevel.id).score);
            PlayerPrefs.SetInt("ID", _currentLevel);
        }

        private IndexState GetState(LevelInfo level)
        {
            if(level.IsUnlocked )
            {
                if (currentLevel == level)
                    return IndexState.UnlockSelected;
                return IndexState.Unlock;
            }
            if (currentLevel == level)
                return IndexState.LockSelected;
            return IndexState.Lock;
        }

        public void StartLevel(LevelInfo level)
        {
            if (!level)
            {
                _currentLevel = 0;
                _currentLevelStars = 0;
                SceneManager.LoadScene(0);
                return;
            }

            _currentLevel = level.id;
            level.Load();

            StartCoroutine(Utils.InvokeAfterUnscaled(AddStuffToEventManager, 1.0f));
        }

        private void OnWin()
        {
            currentLevel.MarkComplete(_currentLevelStars);
        }

        private void AddStuffToEventManager()
        {
            EventManager.instance.onWin.AddListener(OnWin);
            EventManager.instance.onScoreUpdated.AddListener(stars => _currentLevelStars = stars);
        }



        public void Next()
        {
            int index = _currentLevel;
            currentLevel.gameObject.SetActive(false);
            _currentLevel = currentLevel.nextLevel.id;
            currentLevel.gameObject.SetActive(true);

            _indexes[index].ChangeColor(GetState(_levels[index]));
            _indexes[_currentLevel].ChangeColor(GetState(currentLevel));
            SetStar(_save.LoadFromJson(currentLevel.id).score);
            PlayerPrefs.SetInt("ID", _currentLevel);

        }

        public void Previous() 
        {
            int index = _currentLevel;
            currentLevel.gameObject.SetActive(false);
            _currentLevel = currentLevel.previousLevel.id;
            currentLevel.gameObject.SetActive(true);

            _indexes[index].ChangeColor(GetState(_levels[index]));
            _indexes[_currentLevel].ChangeColor(GetState(currentLevel));
            SetStar(_save.LoadFromJson(currentLevel.id).score);
            PlayerPrefs.SetInt("ID", _currentLevel);
        }
    }
}