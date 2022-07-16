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
            lv3MidLevelMusic,
            lv4Music,
            lv5Music,
            lv6Music,
            lv7Music,
            lv8Music;

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
            if (scene == GameScene.LEVEL_4) Play(lv4Music);
            if (scene == GameScene.LEVEL_5) Play(lv5Music);
            if (scene == GameScene.LEVEL_6) Play(lv6Music);
            if (scene == GameScene.LEVEL_7) Play(lv7Music);
            if (scene == GameScene.LEVEL_8) Play(lv8Music);
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

        public void PlayLv4BossMusic()
        {
            PlayLv2BossMusic();
        }

        public void PlayLv5BossMusic()
        {
            PlayLv2BossMusic();
        }

        public void PlayLv6BossMusic1()
        {
            PlayLv2BossMusic();
        }

        public void PlayLv6BossMusic2()
        {
            PlayLv1BossMusic();
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