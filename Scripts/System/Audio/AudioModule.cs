using UnityEngine;
using PurpleFlowerCore.Pool;
namespace PurpleFlowerCore.Audio
{
    public class AudioModule : MonoBehaviour
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

        public void PauseBGM()
        {
            BgmSource.Pause();
        }

        public void RePlayBGM()
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

        #region Effect

        private GameObjectPoolData _pool;

        #endregion
    }
}