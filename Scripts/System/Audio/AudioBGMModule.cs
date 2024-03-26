using UnityEngine;

namespace PurpleFlowerCore.Audio
{
    public class AudioBGMModule : MonoBehaviour
    {
        #region BGM

        private AudioSource _bgmSource;

        private AudioSource BgmSource
        {
            get
            {
                if (_bgmSource is not null) return _bgmSource;
                _bgmSource = gameObject.AddComponent<AudioSource>();
                _bgmSource.loop = true;
                return _bgmSource;
            }
        }
        
        public void PlayBGM(AudioClip clip)
        {
            BgmSource.clip = clip;
            BgmSource.Play();
        }

        public void Pause()
        {
            BgmSource.Pause();
        }

        public void Unpause()
        {
            BgmSource.Play();
        }

        public void ChangeBGMValue(float volume)
        {
            BgmSource.volume = volume;
        }

        
        
        public AudioSource GetAudioSource()
        {
            return BgmSource;
        }
        
        #endregion

       
    }
}