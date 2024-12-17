using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Effect
{
    [RequireComponent(typeof(Light2D))]
    public class FlickeringLight : MonoBehaviour
    {
        [Header("Flicker Settings")]
        [Tooltip("Valeur centrale autour de laquelle le flicker oscille.")]
        [SerializeField] private float _baseValue = 1f;

        [Tooltip("Amplitude maximale du flicker.")]
        [SerializeField] private float _flickerAmplitude = 0.5f;

        [Tooltip("Vitesse du flicker.")]
        [SerializeField] private float _flickerSpeed = 1f;

        private float _randomSeed;

        private float _flickerResult;

        private Light2D _light;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void Start()
        {
            // Initialise le seed pour un flicker unique
            _randomSeed = Random.Range(0f, 1000f);
        }

        private void Update()
        {
            // Calcul de flicker avec du Perlin Noise pour un effet plus naturel
            float noiseValue = Mathf.PerlinNoise(_randomSeed, Time.time * _flickerSpeed);

            // Remap le bruit (0 à 1) vers (-1 à 1) pour osciller autour de la valeur de base
            float flickerOffset = (noiseValue * 2f - 1f) * _flickerAmplitude;

            // Applique l'offset à la valeur de base
            _flickerResult = _baseValue + flickerOffset;
            _light.intensity = _flickerResult;
        }
    }
}