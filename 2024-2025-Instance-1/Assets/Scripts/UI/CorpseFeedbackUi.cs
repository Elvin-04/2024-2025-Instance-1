using System;
using Grid;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Corpse))]
    public class CorpseFeedbackUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text; 
        
        private Corpse _corpse;

        private void Awake()
        {
            _corpse = GetComponent<Corpse>();
        }

        private void Start()
        {
            UpdateUi(_corpse.GetCurrentLifeTime());
            _corpse.onTick += UpdateUi;
        }

        private void UpdateUi(int currentTick)
        {
            _text.text = currentTick.ToString();
        }
    }
}