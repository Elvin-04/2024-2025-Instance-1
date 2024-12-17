using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Effect
{
    [RequireComponent(typeof(Light2D))]
    public class FlickeringLight : MonoBehaviour
    {
        [SerializeField] private float _minIntensity = 0.5f;
        [SerializeField] private float _maxIntensity = 1.5f;
        [SerializeField] private float _flickerSpeed = 10f;
        [SerializeField] private float _randomSpeed = 10f;
        private Light2D _fireLight;

        private Light2D fireLight
        {
            get
            {
                if (_fireLight == null)
                {
                    _fireLight = GetComponent<Light2D>();
                }

                return _fireLight;
            }
        }

        private void Update()
        {
            fireLight.pointLightOuterRadius =
                Mathf.Lerp(_minIntensity, _maxIntensity, Mathf.PingPong(Time.time * _flickerSpeed, 1)) +
                Random.Range(-1f, 1f) * _randomSpeed;
        }
    }
}