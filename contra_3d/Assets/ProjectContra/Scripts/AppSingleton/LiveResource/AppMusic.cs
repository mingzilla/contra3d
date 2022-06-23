using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton.LiveResource
{
    public class AppMusic : MonoBehaviour
    {
        public static AppMusic instance;

        [SerializeField] private AudioSource lv1Music,
            lv1BossMusic,
            lv2Music,
            lv2BossMusic,
            lv3Music,
            lv3MidLevelMusic;

        private AudioSource currentMusic;

        private List<AudioSource> AllMusic()
        {
            return new List<AudioSource>()
            {
                lv1Music,
                lv1BossMusic,
                lv2Music,
                lv2BossMusic,
                lv3Music,
                lv3MidLevelMusic,
            };
        }

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
        }

        public void PlayByScene(GameScene scene)
        {
            if (scene == GameScene.LEVEL_1) Play(lv1Music);
            if (scene == GameScene.LEVEL_2) Play(lv2Music);
            if (scene == GameScene.LEVEL_3) Play(lv3Music);
        }

        public void PlayLv1BossMusic()
        {
            Play(lv1BossMusic);
        }

        public void PlayLv2BossMusic()
        {
            Play(lv2BossMusic);
        }

        public void PlayLv3MidLevelMusic()
        {
            Play(lv3MidLevelMusic);
        }

        public void PlayLv3BossMusic()
        {
            PlayLv2BossMusic();
        }

        public void Play(AudioSource source)
        {
            Stop();
            currentMusic = source;
            currentMusic.Play();
        }

        public void Stop()
        {
            if (currentMusic != null)
            {
                currentMusic.Stop();
                currentMusic = null;
            }
        }

        public void Pause()
        {
            if (currentMusic != null) currentMusic.Pause();
        }

        public void UnPause()
        {
            if (currentMusic != null) currentMusic.UnPause();
        }
    }
}