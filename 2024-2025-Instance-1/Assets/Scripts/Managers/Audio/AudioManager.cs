using System;
using System.Collections.Generic;
using Managers.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Managers.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        [Header("Music Settings")]
        [SerializeField] private Transform _musicParent;

        [SerializeField] private GameObject _prefabMusicLink;
        [SerializeField] private List<AudioClipEntry> _musicEntries = new();

        [Space(30)]
        [Header("SFX Settings")]
        [SerializeField] private Transform _sfxParent;

        [SerializeField] private GameObject _prefabSfxLink;
        [SerializeField] private List<AudioClipEntry> _sfxEntries = new();

        private bool _isSfxPaused;
        private AudioSource _musicAudioSource;
        private AudioSource _sfxAudioSource;

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
            InitPrefabsLink();
            InitListeners();
            SceneManager.sceneLoaded += (_, _) => { InitListeners(); };
        }

        private void Update()
        {
            // Teste si l'on appuie sur S pour mettre en pause ou reprendre les effets sonores
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (_isSfxPaused)
                    UnpauseSfx();
                else
                    PauseSfx();
            }
        }


        private void InitPrefabsLink()
        {
            _musicAudioSource = Instantiate(_prefabMusicLink, _musicParent).GetComponent<AudioSource>();
            _sfxAudioSource = Instantiate(_prefabSfxLink, _sfxParent).GetComponent<AudioSource>();
        }

        /// <summary>
        ///     Adds the listeners to the event manager for audio related events
        /// </summary>
        private void InitListeners()
        {
            // Music
            EventManager.instance.onPlayMusic.AddListener(PlayMusic);
            EventManager.instance.onPauseMusic.AddListener(PauseMusic);
            EventManager.instance.onStopMusic.AddListener(StopMusic);

            // Sfx
            EventManager.instance.onPlaySfx.AddListener(PlaySfx);
            EventManager.instance.onPauseSfx.AddListener(PauseSfx);
            EventManager.instance.onStopSfx.AddListener(StopSfx);
        }

        private AudioClip FindClip(List<AudioClipEntry> entries, SoundsName name)
        {
            AudioClipEntry entry = entries.Find(e => e.key == name);
            if (entry != null && entry.clips.Count > 0) return entry.clips[Random.Range(0, entry.clips.Count)];
            return null;
        }

        #region Music

        private void PlayMusic(SoundsName name)
        {
            AudioClip clip = FindClip(_musicEntries, name);
            if (!clip)
            {
                Debug.LogWarning($"No music clip found for {name}");
                return;
            }

            _musicAudioSource.clip = clip;
            _musicAudioSource.Play();
        }

        private void PauseMusic()
        {
            if (_musicAudioSource.isPlaying) _musicAudioSource.Pause();
        }

        private void UnpauseMusic()
        {
            _musicAudioSource.UnPause();
        }

        private void StopMusic()
        {
            if (!_musicAudioSource.isPlaying) return;
            _musicAudioSource.Stop();
        }

        #endregion

        #region SFX

        private void PlaySfx(SoundsName name)
        {
            AudioClip clip = FindClip(_sfxEntries, name);
            if (!clip)
            {
                Debug.LogWarning($"No SFX clip found for {name}");
                return;
            }

            // Utilise PlayOneShot pour jouer sans écraser le son précédent
            _sfxAudioSource.PlayOneShot(clip);
        }

        private void PauseSfx()
        {
            if (!_sfxAudioSource.isPlaying) return;
            _sfxAudioSource.Pause();
            _isSfxPaused = true;
        }

        private void UnpauseSfx()
        {
            _sfxAudioSource.UnPause();
            _isSfxPaused = false; // Marquer les SFX comme non-pause
        }

        private void StopSfx()
        {
            if (!_sfxAudioSource.isPlaying) return;
            _sfxAudioSource.Stop();
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


/*      //[SerializeField] private float _maxSpacialBlend = 0.95f;
        //private Dictionary<SoundsName, List<AudioClip>> _musicPlayInlist = new();
        //private Dictionary<SoundsName, List<AudioClip>> _sfxPlayInlist = new();

        //private ComponentPoolAudio<AudioSource> _musicPool;
        //private ComponentPoolAudio<AudioSource> _sfxPool;
        //private Camera cam;

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
        ///     Convert a list of AudioClipEntry into a dictionary where the key is the SoundsName and the value is the list of
        ///     AudioClips associated with this SoundsName
        /// </summary>
        /// <param name="entries">The list of AudioClipEntry to convert</param>
        /// <returns>The converted dictionary</returns>
        private Dictionary<SoundsName, List<AudioClip>> ConvertListToDictionary(List<AudioClipEntry> entries)
        {
            Dictionary<SoundsName, List<AudioClip>> dictionary = new();

            foreach (AudioClipEntry entry in entries)
                if (!dictionary.ContainsKey(entry.key))
                    dictionary.Add(entry.key, entry.clips);

            return dictionary;
        }
        #region Music

        /// <summary>
        ///     Ne pas oublier de mettre tous les Audios dans la liste avec sa clef de detection
        /// </summary>
        /// <param name="name"></param>
        public void PlayMusic(SoundsName name, Transform newTransformToPlaySound)
        {
            if (!_musicPlayInlist.TryGetValue(name, out List<AudioClip> clips))
            {
                Debug.LogWarning($"No Music track named {name} found in the playlist.");
                return;
            }

            if (clips.Count == 0)
            {
                Debug.LogWarning($"No clips available for Music track {name}.");
                return;
            }

            AudioClip clip = clips[Random.Range(0, clips.Count)]; // Sélection aléatoire d'un clip
            AudioSource musicSource = newTransformToPlaySound == null ? _musicPool.Get() : _musicPool.Get(newTransformToPlaySound);

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (musicSource == null)
            {
                GameObject musicObject = newTransformToPlaySound == null ? Instantiate(_prefabMusicLink, _musicParent) : Instantiate(_prefabMusicLink, newTransformToPlaySound.position, Quaternion.identity, _musicParent);
                ;
                musicSource = musicObject.GetComponent<AudioSource>();
            }

            // Configurer l'AudioSource avec le clip audio sélectionné
            musicSource.clip = clip;
            musicSource.Play();

            // Initialiser le script AudioSourceRelease pour libérer l'objet après la lecture
            AudioSourceRelease releaseScript = musicSource.gameObject.GetComponent<AudioSourceRelease>() ?? musicSource.gameObject.AddComponent<AudioSourceRelease>();
            releaseScript.Initialize(musicSource, _musicPool);
            Debug.Log($"Playing music: '{clip.name}' from track '{name}'");
        }

        private void PlayAllMusic()
        {
            if (_sfxPlayInlist.Count == 0) return;
            foreach (AudioSource musicSource in _musicPool.GetAll()) musicSource.Play();
        }

        private void PauseMusic(SoundsName name)
        {
            foreach (AudioSource musicSource in _musicPool.GetAll())
            {
                if (musicSource.clip.name != name.ToString()) continue;
                musicSource.Pause();
            }
        }

        private void PauseAllMusic()
        {
            foreach (AudioSource musicSource in _musicPool.GetAll()) musicSource.Pause();
        }

        private void StopMusic(SoundsName name)
        {
            foreach (AudioSource musicSource in _musicPool.GetAll())
            {
                if (musicSource.clip.name != name.ToString()) continue;

                musicSource.Stop();
                _musicPool.Release(musicSource.gameObject);
            }
        }

        private void StopAllMusic()
        {
            foreach (AudioSource musicSource in _musicPool.GetAll())
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
            if (!_sfxPlayInlist.TryGetValue(name, out List<AudioClip> clips))
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

            AudioClip clip = clips[Random.Range(0, clips.Count)]; // Sélection aléatoire d'un clip
            AudioSource sfxSource = newTransformToPlaySound == null ? _sfxPool.Get() : _sfxPool.Get(newTransformToPlaySound);

            // Si le pool est vide, créer un nouvel objet AudioSource
            if (sfxSource == null)
            {
                GameObject sfxObject = newTransformToPlaySound == null ? Instantiate(_prefabSfxLink, _sfxParent) : Instantiate(_prefabSfxLink, newTransformToPlaySound.position, Quaternion.identity, _sfxParent);
                sfxSource = sfxObject.GetComponent<AudioSource>();
            }

            // Configurer l'AudioSource avec le clip audio sélectionné
            sfxSource = sfxSource.GetComponent<AudioSource>();
            sfxSource.spatialBlend = newTransformToPlaySound == null ? 0f : _maxSpacialBlend;
            sfxSource.clip = clip;
            sfxSource.Play();

            // Initialiser le script AudioSourceRelease pour libérer l'objet après la lecture
            AudioSourceRelease releaseScript = sfxSource.gameObject.GetComponent<AudioSourceRelease>() ?? sfxSource.gameObject.AddComponent<AudioSourceRelease>();
            releaseScript.Initialize(sfxSource, _sfxPool);
            //Debug.Log($"Playing sfx: '{clip.name}' from track '{name}' at position {newTransformToPlaySound.position}");
        }

        private bool IsInCameraView(Vector3 position, Camera cam)
        {
            Vector3 viewportPoint = cam.WorldToViewportPoint(position);
            return viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
        }


        private void PlayAllSfx()
        {
            if (_sfxPlayInlist.Count == 0) return;
            foreach (AudioSource sfxSource in _sfxPool.GetAll()) sfxSource.Play();
        }

        private void PauseSfx(SoundsName name)
        {
            foreach (AudioSource sfxSource in _sfxPool.GetAll())
            {
                if (sfxSource.clip.name != name.ToString()) continue;
                sfxSource.Pause();
            }
        }

        private void PauseAllSfx()
        {
            foreach (AudioSource sfxSource in _sfxPool.GetAll()) sfxSource.Pause();
        }


        private void StopSfx(SoundsName name)
        {
            if (_sfxPlayInlist.TryGetValue(name, out List<AudioClip> sfxList))
            {
                foreach (AudioClip audioClip in sfxList)
                {
                    AudioSource sfxSource = GetAudioSourceFromClip(audioClip);
                    if (!sfxSource) continue;
                    sfxSource.Stop();
                    _sfxPool.Release(sfxSource.gameObject);
                }

                // Optionnel : Nettoie la liste après avoir tout arrêté
                Debug.LogWarning("_sfxPlayInlist.count : " + _sfxPlayInlist.Count);
                //_sfxPlayInlist[name].Clear();
            }
        }

        private void StopAllSfx()
        {
            foreach (AudioSource sfxSource in _sfxPool.GetAll())
            {
                sfxSource.Stop();
                _sfxPool.Release(sfxSource.gameObject);
            }
        }

        #endregion

        /*private AudioSource GetAudioSourceFromClip(AudioClip clip)
        {
            foreach (KeyValuePair<SoundsName, List<AudioClip>> source in _sfxPlayInlist.Key)
            {
                if (source.Key .clip == clip)
                    return source;
            }
            return null;
        }#1#*/