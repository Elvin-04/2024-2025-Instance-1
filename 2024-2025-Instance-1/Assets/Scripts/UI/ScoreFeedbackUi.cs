using System;
using Managers;
using Score;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI
{
    public class ScoreFeedbackUi : MonoBehaviour
    {
        [SerializeField] private Image _deathImage;
        [SerializeField] private Image _movementImage;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private float _imageFirstValueThreshold, _imageSecondValueThreshold, _imageThirdValueThreshold;

        private void Awake()
        {
            Assert.IsNotNull(_levelManager, "ScoreManager is not set in ScoreFeedbackUi");
            Assert.IsNotNull(_scoreManager, "ScoreManager is not set in ScoreFeedbackUi");
            Assert.IsNotNull(_deathImage, "DeathImage is not set in ScoreFeedbackUi");
            Assert.IsNotNull(_movementImage, "MovementImage is not set in ScoreFeedbackUi");
        }

        private void Start()
        {
            SetScores(0, 0);
            EventManager.instance.onScoreUpdated.AddListener(OnScoreUpdated);
        }

        private void OnScoreUpdated(int score)
        {
            Debug.Log("score updated");
            SetScores(_scoreManager.deaths, _scoreManager.steps);
        }

        private void SetScores(float deathScore, float movementScore)
        {
            SetDeathScore(deathScore);
            SetMovementScore(movementScore);
        }

        private void SetDeathScore(float score)
        {
            _deathImage.fillAmount = CalculatePercentage(
                (_levelManager.maxDeaths.firstThreshold, _imageFirstValueThreshold),
                (_levelManager.maxDeaths.secondThreshold, _imageSecondValueThreshold),
                (_levelManager.maxDeaths.thirdThreshold, _imageThirdValueThreshold), score);
        }

        private void SetMovementScore(float score)
        {
            _movementImage.fillAmount = CalculatePercentage(
                (_levelManager.maxSteps.firstThreshold, _imageFirstValueThreshold),
                (_levelManager.maxSteps.secondThreshold, _imageSecondValueThreshold),
                (_levelManager.maxSteps.thirdThreshold, _imageThirdValueThreshold), score);
        }

        private static float CalculatePercentage((float, float) t1, (float, float) t2, (float, float) t3, float value)
        {
            (float, float)[] thresholds = { (0, 0), t1, t2, t3 };
            Array.Sort(thresholds, (a, b) => a.Item1.CompareTo(b.Item1));

            if (value >= thresholds[3].Item1)
            {
                return 1;
            }

            for (int i = 0; i < thresholds.Length - 1; i++)
            {
                if (value >= thresholds[i].Item1 && value <= thresholds[i + 1].Item1)
                {
                    float range = thresholds[i + 1].Item1 - thresholds[i].Item1;
                    float valuePosition = value - thresholds[i].Item1;
                    return thresholds[i].Item2 + (valuePosition / range) *
                        (thresholds[i + 1].Item2 - thresholds[i].Item2);
                }
            }

            return 0;
        }
    }
}