using UnityEngine;
using UnityEngine.Audio;

namespace Data
{
    public class AudioSourcePooled : MonoBehaviour, IPooledObject<AudioSourcePooled>
    {
        [SerializeField] private AudioMixer _audioMixer;
        private AudioSource _audioSource;
        private IPool<AudioSourcePooled> _pool;


        public void SetPool(Pool<AudioSourcePooled> pool)
        {
            _pool = pool;
        }

        public void Release()
        {
            _pool.Release(this);
        }
    }
}