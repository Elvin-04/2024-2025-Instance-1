using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreUI : MonoBehaviour
    {
        private TextMeshProUGUI _scoreText;

        private void Awake()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            EventManager.instance.onScoreUpdated?.AddListener(OnScoreUpdated);
        }

        private void OnScoreUpdated(float score)
        {
            _scoreText.text = score.ToString(CultureInfo.CurrentCulture);
        }
    }
}