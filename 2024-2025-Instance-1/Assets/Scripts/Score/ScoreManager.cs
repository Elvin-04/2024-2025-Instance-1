using Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Score
{
    [RequireComponent(typeof(LevelManager))]
    public class ScoreManager : MonoBehaviour
    {
        private int _deaths;
        private LevelManager _levelManager;
        private int _stars;
        private int _steps;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        private void Start()
        {
            EventManager.instance.onClockUpdated.AddListener(OnPlayerMove);
            EventManager.instance.onDeath.AddListener(OnPlayerDies);
            EventManager.instance.onWin.AddListener((() => StartCoroutine(LateCalculate())));
            Invoke(nameof(LateStart), 0);

            EventManager.instance.onWin.AddListener(() => Debug.Log(_steps));
        }

        private IEnumerator LateCalculate()
        {
            yield return new WaitForEndOfFrame();
            CalculateFinalScore();
        }

        private void LateStart()
        {
            CalculateFinalScore();
        }

        private void OnPlayerDies(bool deathEffect)
        {
            _deaths++;
            CalculateFinalScore();
        }

        private void OnPlayerMove()
        {
            _steps++;
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
            if (_deaths < _levelManager.maxDeaths.firstThreshold && _steps < _levelManager.maxSteps.firstThreshold)
            {
                _stars = 3;
            }
            else if (_deaths < _levelManager.maxDeaths.secondThreshold &&
                     _steps < _levelManager.maxSteps.secondThreshold)
            {
                _stars = 2;
            }
            else if (_deaths < _levelManager.maxDeaths.thirdThreshold &&
                     _steps < _levelManager.maxSteps.thirdThreshold)
            {
                _stars = 1;
            }
            else
            {
                _stars = 0;
            }
            Debug.Log("event called");
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