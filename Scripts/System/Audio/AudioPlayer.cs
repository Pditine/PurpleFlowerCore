using UnityEngine;

namespace PurpleFlowerCore.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        public AudioSource AudioSource => _audioSource;
        public void Init()
        {
            var audioSource = GetComponent<AudioSource>();
            _audioSource = audioSource ? audioSource : gameObject.AddComponent<AudioSource>();
            
        }

        public void Play()
        {
            //_audioSource.PlayOneShot();
            //_audioSource.
        }

        public void Destroy()
        {
            
        }
        
        
        
    }
}