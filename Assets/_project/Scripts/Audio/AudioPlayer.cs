using UnityEngine;

namespace _project.Scripts.Audio
{
    public interface IAudioPlayer
    {
        void Play(AudioClip clip);
    }

    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private AudioSource _audioSource;

        public void Play(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}