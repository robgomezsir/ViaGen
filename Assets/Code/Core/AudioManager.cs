using UnityEngine;

namespace ViaGen.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();
        }

        public AudioSource MusicSource => musicSource;

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null) return;
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null || sfxSource == null) return;
            sfxSource.PlayOneShot(clip);
        }
    }
}
