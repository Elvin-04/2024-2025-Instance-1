using Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Score
{
    [RequireComponent(typeof(LevelManager))]
    public class ScoreManager : MonoBehaviour
    {
        public int deaths { get; private set; }
        private LevelManager _levelManager;
        private int _stars;
        public int steps { get; private set; }

        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        private void Start()
        {
            EventManager.instance.onClockUpdated.AddListener(OnPlayerMove);
            EventManager.instance.onDeath.AddListener(OnPlayerDies);
            EventManager.instance.onWin.AddListener((() => StartCoroutine(LateCalculate())));

            EventManager.instance.onWin.AddListener(() => Debug.Log(steps));
            EventManager.instance.onWin.AddListener(() =>
            {
                Save save = new Save();
                SaveObject nextLevel = new SaveObject(PlayerPrefs.GetInt("ID") + 1, 0);
                save.SaveToJson(nextLevel);
                SaveObject currentLevel = new SaveObject(PlayerPrefs.GetInt("ID"), _stars);
                save.SaveToJson(currentLevel);
            });
        }

        private IEnumerator LateCalculate()
        {
            yield return new WaitForEndOfFrame();
            CalculateFinalScore();
        }

        private void OnPlayerDies(bool deathEffect)
        {
            deaths++;
            CalculateFinalScore();
        }

        private void OnPlayerMove()
        {
            steps++;
            CalculateFinalScore();
        }

        private void CalculateFinalScore()
        {
            CalculateScore();
            EventManager.instance.onScoreUpdated?.Invoke(_stars);
        }

        private void CalculateScore()
        {
            _stars = 3;
            if (deaths < _levelManager.maxDeaths.firstThreshold && steps < _levelManager.maxSteps.firstThreshold)
            {
                _stars = 3;
            }
            else if (deaths < _levelManager.maxDeaths.secondThreshold &&
                     steps < _levelManager.maxSteps.secondThreshold)
            {
                _stars = 2;
            }
            else if (deaths < _levelManager.maxDeaths.thirdThreshold &&
                     steps < _levelManager.maxSteps.thirdThreshold)
            {
                _stars = 1;
            }
            else
            {
                _stars = 0;
            }
        }
    }
}

[Serializable]
public class ScoreCounter
{
    [field: SerializeField] public int firstThreshold { get; private set; }
    [field: SerializeField] public int secondThreshold { get; private set; }
    [field: SerializeField] public int thirdThreshold { get; private set; }
}