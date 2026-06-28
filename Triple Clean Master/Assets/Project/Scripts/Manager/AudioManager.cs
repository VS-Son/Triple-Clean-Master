using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Manager
{
    public enum AudioType
    {
        BGM,
        Sfx
    }
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;
        [Range(0, 1f)] public float bgmVolume;
        [Range(0, 1f)] public float sfxVolume;
        private const string BgmMutedKey = "BgmMuted";
        private const string SfxMutedKey = "SfxMuted";
        private readonly Dictionary<string, AudioClip> _audioClips = new();

        private void Awake()
        {
            bgmSource.mute = PlayerPrefs.GetInt(BgmMutedKey, false ? 1 : 0) ==1 ;
            sfxSource.mute = PlayerPrefs.GetInt(SfxMutedKey, false ? 1 : 0) == 1;
            LoadAllSfx();
        }

        private void LoadAllSfx()
        {
            AudioClip[] audioClips = UnityEngine.Resources.LoadAll<AudioClip>("Audio");
            foreach (var clip in audioClips)
            {
                var nameClip = clip.name.ToLower();
                _audioClips[nameClip] = clip;
            }
        }

        private void OnValidate()
        {
            bgmSource.volume = bgmVolume;
            sfxSource.volume = sfxVolume;
        }

        public void PlayBGM(string nameBGM, float duration)
        {
            nameBGM = nameBGM.ToLower();
            if (_audioClips.TryGetValue(nameBGM, out var clip))
            {
                bgmSource.clip = clip;
                bgmSource.loop = true;
                bgmSource.Play();
                bgmSource.volume = bgmVolume;
                bgmSource.DOFade(bgmVolume, duration);
            }
            else
            {
                Debug.LogError($"BGM not found: {nameBGM}");
            }
        }

        public void StopBGM()
        {
            bgmSource.Stop();
        }

        public void PlaySfx(string nameSfx)
        {
            nameSfx = nameSfx.ToLower();
            if (_audioClips.TryGetValue(nameSfx, out var clip))
            {
                sfxSource.volume = sfxVolume;
                sfxSource.PlayOneShot(clip, sfxVolume);
            }
            else
            {
                Debug.LogError($"Sfx not found: {nameSfx}");
            }
        }

        public void ToggleMute(bool isMute, AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.BGM:
                    bgmSource.mute = isMute;
                    PlayerPrefs.SetInt(BgmMutedKey, isMute ? 1: 0);
                    PlayerPrefs.Save();
                    break;
                case AudioType.Sfx:
                    sfxSource.mute = isMute;
                    PlayerPrefs.SetInt(SfxMutedKey, isMute ?1 :0);
                    PlayerPrefs.Save();

                    break;
            }
        }
        public bool IsMuted(AudioType audioType)
        {
            return audioType switch
            {
                AudioType.BGM => bgmSource.mute,
                AudioType.Sfx => sfxSource.mute,
                _ => false
            };
        }
        
    }
}
