using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Managers.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("Music")]
        [SerializeField] private Transform _musicParent;

        [SerializeField] private GameObject _prefabMusicLink;
        [SerializeField] private List<AudioClipEntry> _musicPlaylistEntries = new();

        [Space(25)]
        [Header("Sfx")]
        [SerializeField] private Transform _sfxParent;

        [SerializeField] private GameObject _prefabSfxLink;
        [SerializeField] private List<AudioClipEntry> _sfxPlaylistEntries = new();

        private Dictionary<string, AudioClip> _musicPlayInlist = new();
        private ComponentPoolAudio<AudioSource> _musicPool;

        private Dictionary<string, AudioClip> _sfxPlayInlist = new();
        private ComponentPoolAudio<AudioSource> _sfxPool;


        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Création avec le système de pool de :
            _musicPool = new ComponentPoolAudio<AudioSource>(_prefabMusicLink, _musicParent, 5); // Pré-allocation de 5 AudioSource pour la musique
            _sfxPool = new ComponentPoolAudio<AudioSource>(_prefabSfxLink, _sfxParent, 10); // Pré-allocation de 10 AudioSource pour les SFX

            // Convertir les listes en dictionnaires
            _musicPlayInlist = ConvertListToDictionary(_musicPlaylistEntries);
            _sfxPlayInlist = ConvertListToDictionary(_sfxPlaylistEntries);
        }

        private Dictionary<string, AudioClip> ConvertListToDictionary(List<AudioClipEntry> entries)
        {
            Dictionary<string, AudioClip> dictionary = new();
            foreach (var entry in entries)
                if (!dictionary.ContainsKey(entry.key))
                    dictionary.Add(entry.key, entry.clip);

            return dictionary;
        }

        #region Music

        /// <summary>
        ///     Ne pas oublier de mettre tous les Audios dans la liste avec sa clef de detection
        /// </summary>
        /// <param name="name"></param>
        public void PlayMusic(string name)
        {
            if (!_musicPlayInlist.ContainsKey(name))
            {
                Debug.LogWarning($"No Music track named {name} found in the playlist.");
                return;
            }

            var musicSource = _musicPool.Get();

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (musicSource == null)
            {
                var musicObject = Instantiate(_prefabMusicLink, _musicParent);
                musicSource = musicObject.GetComponent<AudioSource>();
            }

            // Configurer l'AudioSource avec le clip audio correspondant
            musicSource.clip = _musicPlayInlist[name];
            musicSource.Play();

            // Initialiser le script AudioSourceRelease pour libérer l'objet après la lecture
            var releaseScript = musicSource.gameObject.AddComponent<AudioSourceRelease>();
            releaseScript.Initialize(musicSource, _musicPool);
        }

        public void PlayAllMusic()
        {
            if (_sfxPlayInlist.Count == 0) return;
            foreach (var musicSource in _musicPool.GetAll()) musicSource.Play();
        }

        public void PauseMusic(string name)
        {
            foreach (var musicSource in _musicPool.GetAll())
            {
                if (musicSource.clip.name != name) continue;
                musicSource.Pause();
            }
        }

        public void PauseAllMusic()
        {
            foreach (var musicSource in _musicPool.GetAll()) musicSource.Pause();
        }

        public void StopMusic(string name)
        {
            foreach (var musicSource in _musicPool.GetAll())
            {
                if (musicSource.clip.name != name) continue;

                musicSource.Stop();
                _musicPool.Release(musicSource.gameObject);
            }
        }

        public void StopAllMusic()
        {
            foreach (var musicSource in _musicPool.GetAll())
            {
                musicSource.Stop();
                _musicPool.Release(musicSource.gameObject);
            }
        }

        #endregion

        #region Sfx

        /// <summary>
        ///     Ne pas oublier de mettre tous les Audios dans la liste avec sa clef de detection
        /// </summary>
        /// <param name="name"></param>
        public void PlaySfx(string name)
        {
            if (!_sfxPlayInlist.ContainsKey(name))
            {
                Debug.LogWarning($"No SFX track named {name} found in the playlist.");
                return;
            }

            var sfxSource = _sfxPool.Get();

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (sfxSource == null)
            {
                var sfxObject = Instantiate(_prefabSfxLink, _sfxParent);
                sfxSource = sfxObject.GetComponent<AudioSource>();
            }

            // Configurer l'AudioSource avec le clip audio correspondant
            sfxSource.clip = _sfxPlayInlist[name];
            sfxSource.Play();

            // Initialiser le script AudioSourceRelease pour libérer l'objet après la lecture
            var releaseScript = sfxSource.gameObject.AddComponent<AudioSourceRelease>();
            releaseScript.Initialize(sfxSource, _sfxPool);
        }

        public void PlayAllSfx()
        {
            if (_sfxPlayInlist.Count == 0) return;
            foreach (var sfxSource in _sfxPool.GetAll()) sfxSource.Play();
        }

        public void PauseSfx(string name)
        {
            foreach (var sfxSource in _sfxPool.GetAll())
            {
                if (sfxSource.clip.name != name) continue;
                sfxSource.Pause();
            }
        }

        public void PauseAllSfx()
        {
            foreach (var sfxSource in _sfxPool.GetAll()) sfxSource.Pause();
        }

        public void StopSfx(string name)
        {
            foreach (var sfxSource in _sfxPool.GetAll())
            {
                if (sfxSource.clip.name != name) continue;

                sfxSource.Stop();
                _sfxPool.Release(sfxSource.gameObject);
            }
        }

        public void StopAllSfx()
        {
            foreach (var sfxSource in _sfxPool.GetAll())
            {
                sfxSource.Stop();
                _sfxPool.Release(sfxSource.gameObject);
            }
        }

        #endregion
    }
}


[Serializable]
public class AudioClipEntry
{
    public string key;
    public AudioClip clip;
}