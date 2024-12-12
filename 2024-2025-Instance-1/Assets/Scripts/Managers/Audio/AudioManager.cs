using System;
using System.Collections.Generic;
using Data;
using Managers.Audio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        [Header("Music")]
        [SerializeField] private Transform _musicParent;

        [SerializeField] private GameObject _prefabMusicLink;
        [SerializeField] private List<AudioClipEntry> _musicPlaylistEntries = new();

        [Space(25)]
        [Header("Sfx")]
        [SerializeField] private Transform _sfxParent;

        [SerializeField] private GameObject _prefabSfxLink;
        [SerializeField] private List<AudioClipEntry> _sfxPlaylistEntries = new();

        private Dictionary<SoundsName, List<AudioClip>> _musicPlayInlist = new();
        private ComponentPoolAudio<AudioSource> _musicPool;

        private Dictionary<SoundsName, List<AudioClip>> _sfxPlayInlist = new();
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
            InitPools();
            InitListeners();
        }

        private void InitPools()
        {
            // Création avec le système de pool de :
            _musicPool = new ComponentPoolAudio<AudioSource>(_prefabMusicLink, _musicParent, 5); // Pré-allocation de 5 AudioSource pour la musique
            _sfxPool = new ComponentPoolAudio<AudioSource>(_prefabSfxLink, _sfxParent, 10); // Pré-allocation de 10 AudioSource pour les SFX

            // Convertir les listes en dictionnaires
            _musicPlayInlist = ConvertListToDictionary(_musicPlaylistEntries);
            _sfxPlayInlist = ConvertListToDictionary(_sfxPlaylistEntries);
        }

        private void InitListeners()
        {
            EventManager.instance.onPlayMusic.AddListener(PlayMusic);
            EventManager.instance.onPlayAllMusic.AddListener(PlayAllMusic);
            EventManager.instance.onPauseMusic.AddListener(PauseMusic);
            EventManager.instance.onPauseAllMusic.AddListener(PauseAllMusic);
            EventManager.instance.onStopMusic.AddListener(StopMusic);
            EventManager.instance.onStopAllMusic.AddListener(StopAllMusic);

            EventManager.instance.onPlaySfx.AddListener(PlaySfx);
            EventManager.instance.onPlayAllSfx.AddListener(PlayAllSfx);
            EventManager.instance.onPauseSfx.AddListener(PauseSfx);
            EventManager.instance.onPauseAllSfx.AddListener(PauseAllSfx);
            EventManager.instance.onStopSfx.AddListener(StopSfx);
            EventManager.instance.onStopAllSfx.AddListener(StopAllSfx);
        }

        private Dictionary<SoundsName, List<AudioClip>> ConvertListToDictionary(List<AudioClipEntry> entries)
        {
            Dictionary<SoundsName, List<AudioClip>> dictionary = new();
            foreach (var entry in entries)
                if (!dictionary.ContainsKey(entry.key))
                    dictionary.Add(entry.key, entry.clips);

            return dictionary;
        }


        #region Music

        /// <summary>
        ///     Ne pas oublier de mettre tous les Audios dans la liste avec sa clef de detection
        /// </summary>
        /// <param name="name"></param>
        public void PlayMusic(SoundsName name)
        {
            if (!_musicPlayInlist.TryGetValue(name, out var clips))
            {
                Debug.LogWarning($"No Music track named {name} found in the playlist.");
                return;
            }

            if (clips.Count == 0)
            {
                Debug.LogWarning($"No clips available for Music track {name}.");
                return;
            }

            var clip = clips[Random.Range(0, clips.Count)]; // Sélection aléatoire d'un clip

            var musicSource = _musicPool.Get();

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (musicSource == null)
            {
                var musicObject = Instantiate(_prefabMusicLink, _musicParent);
                musicSource = musicObject.GetComponent<AudioSource>();
            }

            // Configurer l'AudioSource avec le clip audio sélectionné
            musicSource.clip = clip;
            musicSource.Play();

            // Initialiser le script AudioSourceRelease pour libérer l'objet après la lecture
            var releaseScript = musicSource.gameObject.GetComponent<AudioSourceRelease>() ?? musicSource.gameObject.AddComponent<AudioSourceRelease>();
            releaseScript.Initialize(musicSource, _musicPool);
            Debug.Log($"Playing music: '{clip.name}' from track '{name}'");
        }

        private void PlayAllMusic()
        {
            if (_sfxPlayInlist.Count == 0) return;
            foreach (var musicSource in _musicPool.GetAll()) musicSource.Play();
        }

        private void PauseMusic(SoundsName name)
        {
            foreach (var musicSource in _musicPool.GetAll())
            {
                if (musicSource.clip.name != name.ToString()) continue;
                musicSource.Pause();
            }
        }

        private void PauseAllMusic()
        {
            foreach (var musicSource in _musicPool.GetAll()) musicSource.Pause();
        }

        private void StopMusic(SoundsName name)
        {
            foreach (var musicSource in _musicPool.GetAll())
            {
                if (musicSource.clip.name != name.ToString()) continue;

                musicSource.Stop();
                _musicPool.Release(musicSource.gameObject);
            }
        }

        private void StopAllMusic()
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
        public void PlaySfx(SoundsName name)
        {
            if (!_sfxPlayInlist.TryGetValue(name, out var clips))
            {
                Debug.LogWarning($"No Music track named {name} found in the playlist.");
                return;
            }

            if (clips.Count == 0)
            {
                Debug.LogWarning($"No clips available for Music track {name}.");
                return;
            }

            var clip = clips[Random.Range(0, clips.Count)]; // Sélection aléatoire d'un clip

            var sfxSource = _sfxPool.Get();

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (sfxSource == null)
            {
                var sfxObject = Instantiate(_prefabSfxLink, _sfxParent);
                sfxSource = sfxObject.GetComponent<AudioSource>();
            }

            // Configurer l'AudioSource avec le clip audio sélectionné
            sfxSource.clip = clip;
            sfxSource.Play();

            // Initialiser le script AudioSourceRelease pour libérer l'objet après la lecture
            var releaseScript = sfxSource.gameObject.GetComponent<AudioSourceRelease>() ?? sfxSource.gameObject.AddComponent<AudioSourceRelease>();
            releaseScript.Initialize(sfxSource, _sfxPool);
            Debug.Log($"Playing sfx: '{clip.name}' from track '{name}'");
        }

        private void PlayAllSfx()
        {
            if (_sfxPlayInlist.Count == 0) return;
            foreach (var sfxSource in _sfxPool.GetAll()) sfxSource.Play();
        }

        private void PauseSfx(SoundsName name)
        {
            foreach (var sfxSource in _sfxPool.GetAll())
            {
                if (sfxSource.clip.name != name.ToString()) continue;
                sfxSource.Pause();
            }
        }

        private void PauseAllSfx()
        {
            foreach (var sfxSource in _sfxPool.GetAll()) sfxSource.Pause();
        }

        private void StopSfx(SoundsName name)
        {
            foreach (var sfxSource in _sfxPool.GetAll())
            {
                if (sfxSource.clip.name != name.ToString()) continue;

                sfxSource.Stop();
                _sfxPool.Release(sfxSource.gameObject);
            }
        }

        private void StopAllSfx()
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
    public SoundsName key;
    public List<AudioClip> clips = new();
}