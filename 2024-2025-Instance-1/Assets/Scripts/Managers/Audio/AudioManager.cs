using Data;
using Managers.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private float _maxSpacialBlend = 0.95f;

        [SerializeField] private Transform _sfxParent;

        [SerializeField] private GameObject _prefabSfxLink;
        [SerializeField] private List<AudioClipEntry> _sfxPlaylistEntries = new();
        private Dictionary<SoundsName, List<AudioClip>> _musicPlayInlist = new();
        private ComponentPoolAudio<AudioSource> _musicPool;

        private Dictionary<SoundsName, List<AudioClip>> _sfxPlayInlist = new();
        private ComponentPoolAudio<AudioSource> _sfxPool;

        private UnityEngine.Camera cam;


        private void Awake()
        {
            cam = Camera.main;
            SceneManager.sceneLoaded += (_, _) =>
            {
                cam = Camera.main;
            };
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
            SceneManager.sceneLoaded += (_, _) =>
            {
                InitPools();
                InitListeners();
            };
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

        /// <summary>
        ///     Adds the listeners to the event manager for audio related events
        /// </summary>
        private void InitListeners()
        {
            // Music
            EventManager.instance.onPlayMusic.AddListener(PlayMusic);
            EventManager.instance.onPlayAllMusic.AddListener(PlayAllMusic);
            EventManager.instance.onPauseMusic.AddListener(PauseMusic);
            EventManager.instance.onPauseAllMusic.AddListener(PauseAllMusic);
            EventManager.instance.onStopMusic.AddListener(StopMusic);
            EventManager.instance.onStopAllMusic.AddListener(StopAllMusic);

            // Sfx
            EventManager.instance.onPlaySfx.AddListener(PlaySfx);
            EventManager.instance.onPlayAllSfx.AddListener(PlayAllSfx);
            EventManager.instance.onPauseSfx.AddListener(PauseSfx);
            EventManager.instance.onPauseAllSfx.AddListener(PauseAllSfx);
            EventManager.instance.onStopSfx.AddListener(StopSfx);
            EventManager.instance.onStopAllSfx.AddListener(StopAllSfx);
        }

        /// <summary>
        ///     Convert a list of AudioClipEntry into a dictionary where the key is the SoundsName and the value is the list of
        ///     AudioClips associated with this SoundsName
        /// </summary>
        /// <param name="entries">The list of AudioClipEntry to convert</param>
        /// <returns>The converted dictionary</returns>
        private Dictionary<SoundsName, List<AudioClip>> ConvertListToDictionary(List<AudioClipEntry> entries)
        {
            // Create a new dictionary to store the result
            Dictionary<SoundsName, List<AudioClip>> dictionary = new();

            // Iterate over the list of entries
            foreach (var entry in entries)
                // If the dictionary does not contain the key, add it
                if (!dictionary.ContainsKey(entry.key))
                    dictionary.Add(entry.key, entry.clips);

            // Return the converted dictionary
            return dictionary;
        }


        #region Music

        /// <summary>
        ///     Ne pas oublier de mettre tous les Audios dans la liste avec sa clef de detection
        /// </summary>
        /// <param name="name"></param>
        public void PlayMusic(SoundsName name, Transform newTransformToPlaySound)
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
            var musicSource = newTransformToPlaySound == null ? _musicPool.Get() : _musicPool.Get(newTransformToPlaySound);

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (musicSource == null)
            {
                var musicObject = newTransformToPlaySound == null ? Instantiate(_prefabMusicLink, _musicParent) : Instantiate(_prefabMusicLink, newTransformToPlaySound.position, Quaternion.identity, _musicParent);
                ;
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
        /// <summary>
        ///     Joue un son d'effet SFX. Cherche dans la liste _sfxPlayInlist la liste des clips audio
        ///     associés au nom de la piste SFX passée en paramètre. Si la liste est vide, ne fait rien.
        /// </summary>
        /// <param name="name">Nom de la piste SFX</param>
        /// <param name="newTransformToPlaySound">Transform à utiliser pour instancier l'objet AudioSource</param>
        public void PlaySfx(SoundsName name, Transform newTransformToPlaySound)
        {
            if (!_sfxPlayInlist.TryGetValue(name, out var clips))
            {
                Debug.LogWarning($"No SFX track named {name} found in the playlist.");
                return;
            }

            if (clips.Count == 0)
            {
                Debug.LogWarning($"No clips available for SFX track {name}.");
                return;
            }

            // check si newTransformToPlaySound est en hors de la zone de camera {return}
            if (newTransformToPlaySound != null && !IsInCameraView(newTransformToPlaySound.position, cam))
                //Debug.LogWarning($"SFX {name} will not be played because {newTransformToPlaySound.name} is out of camera view.");
                return;

            var clip = clips[Random.Range(0, clips.Count)]; // Sélection aléatoire d'un clip
            var sfxSource = newTransformToPlaySound == null ? _sfxPool.Get() : _sfxPool.Get(newTransformToPlaySound);

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (sfxSource == null)
            {
                var sfxObject = newTransformToPlaySound == null ? Instantiate(_prefabSfxLink, _sfxParent) : Instantiate(_prefabSfxLink, newTransformToPlaySound.position, Quaternion.identity, _sfxParent);
                sfxSource = sfxObject.GetComponent<AudioSource>();
            }

            // Configurer l'AudioSource avec le clip audio sélectionné
            sfxSource = sfxSource.GetComponent<AudioSource>();
            sfxSource.spatialBlend = newTransformToPlaySound == null ? 0f : _maxSpacialBlend;
            sfxSource.clip = clip;
            sfxSource.Play();

            // Initialiser le script AudioSourceRelease pour libérer l'objet après la lecture
            var releaseScript = sfxSource.gameObject.GetComponent<AudioSourceRelease>() ?? sfxSource.gameObject.AddComponent<AudioSourceRelease>();
            releaseScript.Initialize(sfxSource, _sfxPool);
            //Debug.Log($"Playing sfx: '{clip.name}' from track '{name}' at position {newTransformToPlaySound.position}");
        }

        private bool IsInCameraView(Vector3 position, UnityEngine.Camera cam)
        {
            Vector3 viewportPoint = cam.WorldToViewportPoint(position);
            return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
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