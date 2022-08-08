using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton.LiveResource
{
    public class AppMusic : MonoBehaviour
    {
        public static AppMusic instance;

        [SerializeField] private AudioSource
            lv1Intro,
            lv1Music,
            lv1BossIntro,
            lv1BossMusic,
            lv2Intro,
            lv2Music,
            lv2BossIntro,
            lv2BossMusic,
            lv3Music,
            lv3MidLevelIntro,
            lv3MidLevelMusic,
            lv4Music,
            lv5Intro,
            lv5Music,
            lv6Music,
            lv8Music,
            endingMusic;

        private List<AudioSource> All()
        {
            return new List<AudioSource>()
            {
                lv1Intro,
                lv1Music,
                lv1BossIntro,
                lv1BossMusic,
                lv2Intro,
                lv2Music,
                lv2BossIntro,
                lv2BossMusic,
                lv3Music,
                lv3MidLevelIntro,
                lv3MidLevelMusic,
                lv4Music,
                lv5Intro,
                lv5Music,
                lv6Music,
                lv8Music,
                endingMusic
            };
        }

        private AudioSource pausedMusic;

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
        }

        public void PlayByScene(GameScene scene)
        {
            if (scene == GameScene.LEVEL_1) PlayIntroAndLoop(lv1Intro, lv1Music);
            if (scene == GameScene.LEVEL_2) PlayIntroAndLoop(lv2Intro, lv2Music);
            if (scene == GameScene.LEVEL_3) Play(lv3Music);
            if (scene == GameScene.LEVEL_4) Play(lv4Music);
            if (scene == GameScene.LEVEL_5) PlayIntroAndLoop(lv5Intro, lv5Music);
            if (scene == GameScene.LEVEL_6) Play(lv6Music);
            if (scene == GameScene.LEVEL_7) Play(lv4Music);
            if (scene == GameScene.LEVEL_8) Play(lv8Music);
        }

        public void PlayLv1BossMusic()
        {
            PlayIntroAndLoop(lv1BossIntro, lv1BossMusic);
        }

        public void PlayLv2BossMusic()
        {
            PlayIntroAndLoop(lv2BossIntro, lv2BossMusic);
        }

        public void PlayLv3MidLevelMusic()
        {
            PlayIntroAndLoop(lv3MidLevelIntro, lv3MidLevelMusic);
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

        public void PlayLv7BossMusic1()
        {
            PlayLv2BossMusic();
        }

        public void PlayLv8BossMusic1()
        {
            PlayLv1BossMusic();
        }

        public void PlayEndingMusic()
        {
            Play(endingMusic);
        }

        public void Play(AudioSource source)
        {
            Stop();
            source.Play();
        }

        public void PlayIntroAndLoop(AudioSource intro, AudioSource loop)
        {
            Stop();
            intro.loop = false;
            loop.loop = true;
            intro.Play();
            // don't do "loop.PlayDelayed(intro.clip.length)", otherwise timescale = 0 doesn't pause the delay
            UnityFn.SetTimeout(this, intro.clip.length, loop.Play);
        }

        public void Stop()
        {
            pausedMusic = null;
            All().ForEach(x =>
            {
                if (x != null) x.Stop();
            });
        }

        public void Pause()
        {
            pausedMusic = All().Find(x => x != null && x.isPlaying);
            if (pausedMusic != null) pausedMusic.Pause();
        }

        public void UnPause()
        {
            if (pausedMusic != null) pausedMusic.UnPause();
        }
    }
}