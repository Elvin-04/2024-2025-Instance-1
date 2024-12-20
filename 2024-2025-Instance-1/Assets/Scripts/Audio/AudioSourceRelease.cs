using System.Collections;
using UnityEngine;

public class AudioSourceRelease : MonoBehaviour
{
    private AudioSource _audioSource;
    private GameObject _objectToRelease;
    private ComponentPoolAudio<AudioSource> _pool; // Référence au pool pour libérer l'objet

    public void Initialize(AudioSource audioSource, ComponentPoolAudio<AudioSource> pool)
    {
        _audioSource = audioSource;
        _pool = pool;
        _objectToRelease = audioSource.gameObject;
        StartCoroutine(ReleaseAfterAudioEnds());
    }

    private IEnumerator ReleaseAfterAudioEnds()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        _pool.Release(_objectToRelease);
    }
}